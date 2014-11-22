using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace OrderDocumentsMonitor
{
    public partial class FileMonitor : ServiceBase
    {
        //Class variables
        private string orderInboxPath;
        private string fileErrorPath;
        private string orderInProgressPath;
        private string emailOrphanPath;
        private OAMEntities1 db;





        public FileMonitor()
        {
            InitializeComponent();

            try
            {

                db = new OAMEntities1();

                //Add Inbox Path of Order Files
                var inboxPath = db.Settings.Where(c => c.SettingId == "OrderInboxPath").FirstOrDefault();
                this.fswOrderInbox.Path = inboxPath.Value;


                //Add In Progress Path
                var inProgressPath = db.Settings.Where(c => c.SettingId == "OrderInProgressPath").FirstOrDefault(); 

                //Add File Error Path
                var errorPath = db.Settings.Where(c => c.SettingId == "FileErrorPath").FirstOrDefault(); 

                //Add email orphan Path
                var emailOphan = db.Settings.Where(c => c.SettingId == "EmailOrphanPath").FirstOrDefault(); 


                //validate existing paths
                if (inboxPath == null || inProgressPath == null || errorPath == null || emailOphan == null)
                {
                    throw new Exception("File Path not found in config database");
                }

                //set file paths
                this.orderInboxPath = inboxPath.Value;
                this.orderInProgressPath = inProgressPath.Value;
                this.fileErrorPath = errorPath.Value;
                this.emailOrphanPath = emailOphan.Value;

                //start file system watcher


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override void OnStart(string[] args)
        {





        }

        protected override void OnStop()
        {
        }

        private void fswOrderInbox_Created(object sender, System.IO.FileSystemEventArgs e)
        {

            try 
	        {
                System.Threading.Thread.Sleep(2000);
                //load document
                var doc = XDocument.Load(e.FullPath);
            
                //validate structure
                var result = doc.XPathSelectElements("//GobyMonitor/Document/Fields/Field").ToList();

                if (result.Count > 0)
                {

                
                    //query order number
                    int orderNumber = Convert.ToInt32(doc.XPathSelectElements("//Field[Name[.=\"Order No\"]]/Data").ToList()[0].Value);

                    //query xml for filename
                    var fileName = doc.XPathSelectElements("//Filespec").ToList()[0].Value;

                    //try 5 times waiting for 10 seconds
                    for (int i = 0; i < 3; i++)
                    {
                        if (!File.Exists(this.orderInboxPath + "\\" + fileName))
                        {
                            System.Threading.Thread.Sleep(10000);
                        }
                    }

                    if (!File.Exists(this.orderInboxPath + "\\" + fileName))
                        throw new FileNotFoundException(String.Format("File {0} not found.", fileName));

                    //query db for existing order
                    var existingOrder = db.Orders.Where(c => c.OrderNumber == orderNumber).FirstOrDefault();  //DBOrders.GetByOrderNumber(orderNumber);


                    //Orders class
                    Order currentOrder;
                    OrderFile currentFile;

                    bool hasOrder = false;

                    if (existingOrder != null) 
                    {
                        currentOrder = existingOrder;
                        hasOrder = true;

                        //check if file exists to add up version and replace file
                        var existingFile = db.OrderFiles.Where(c => c.OrderNumber == orderNumber && c.FileName == fileName).FirstOrDefault(); 

                        if (existingFile != null)
                        {
                            
                            currentFile = existingFile;
                            currentFile.Version++;
                            currentFile.XMLFilePath = e.Name;

                            OrderHistory h1 = new OrderHistory();
                            h1.ActionBy = "OAM System";
                            h1.ActionDate = DateTime.Now;
                            h1.ActionName = "File updated to order.";
                            h1.ActionValue = String.Format("File {0} has been updated to version {1} on the order.", currentFile.FileName, currentFile.Version);
                            h1.Order = currentOrder;
                            h1.OrderStatusId = currentOrder.StatusId;

                            db.OrderHistories.Add(h1);

                        }
                        else
                        {
                            //order exists but the file doesn't
                            currentFile = new OrderFile();
                            AddFileToOrder(currentOrder, currentFile, doc, e.FullPath);

                            OrderHistory h1 = new OrderHistory();
                            h1.ActionBy = "OAM System";
                            h1.ActionDate = DateTime.Now;
                            h1.ActionName = "File added to order.";
                            h1.ActionValue = String.Format("File {0} has been added to the order.", currentFile.FileName);
                            h1.Order = currentOrder;
                            h1.OrderStatusId = currentOrder.StatusId;

                            db.OrderHistories.Add(h1);
                        }

                    }
                    else
                    {
                        //order does not exist
                        currentOrder = new Order();
                        currentFile = new OrderFile();


                        //check if its an email.
                        var fName = doc.XPathSelectElements("//Filespec").ToList()[0].Value;
                        if (fName.ToLower().Contains(".msg"))
                        {
                            //is email then do not create order and move the file.

                        }
                        else
                        {
                            //create order and assotiate file.
                            

                            //add order to database
                            this.AddToOrder(currentOrder, doc);

                            hasOrder = true;

                            //add history order created
                            OrderHistory h = new OrderHistory();
                            h.ActionBy = "OAM System";
                            h.ActionDate = DateTime.Now;
                            h.ActionName = "Order Creation";
                            h.ActionValue = "Order created in the system.";
                            h.Order = currentOrder;
                            h.OrderStatusId = 1;

                            db.OrderHistories.Add(h);

                            //add new file
                            AddFileToOrder(currentOrder, currentFile, doc, e.Name);

                            OrderHistory h1 = new OrderHistory();
                            h1.ActionBy = "OAM System";
                            h1.ActionDate = DateTime.Now;
                            h1.ActionName = "File added to order.";
                            h1.ActionValue = String.Format("File {0} has been added to the order.", currentFile.FileName);
                            h1.Order = currentOrder;
                            h1.OrderStatusId = 1;
                        }
                    }

                    //call add or update
                    db.SaveChanges();

                    //move file to In Progress
                    
                    //documentation
                    if (hasOrder)
                    {
                        FileMove(this.orderInboxPath + "\\" + currentFile.FileName, this.orderInProgressPath + "\\" + currentFile.FileName);


                        //XML File
                        FileMove(this.orderInboxPath + "\\" + e.Name, this.orderInProgressPath + "\\" + e.Name);
                    }
                    else
                    {
                        //Email orphan move to new folder

                        FileMove(this.orderInboxPath + "\\" + doc.XPathSelectElements("//Filespec").ToList()[0].Value, this.emailOrphanPath + "\\" + doc.XPathSelectElements("//Filespec").ToList()[0].Value);


                        //XML File
                        FileMove(this.orderInboxPath + "\\" + e.Name, this.emailOrphanPath + "\\" + e.Name);
                    }
                    

                    //log file event to history table
                    //this.WriteToLog("File format error.");
               
                }
                else 
                {
                    //move to file error folder.
                    FileMove(this.orderInboxPath + "\\" + e.Name, this.fileErrorPath + "\\" + e.Name);
                  
                    
                }
            
            }
	        catch (Exception ex)
	        {
                FileMove(this.orderInboxPath + "\\" + e.Name, this.fileErrorPath + "\\" + e.Name);
                this.WriteToLog(ex.Message + " " + ex.StackTrace);
	        }
            
        }

        private void FileMove(string source, string destination)
        {
            
            if (File.Exists(destination))
            {
                File.Delete(destination);
            }

            File.Move(source, destination);

        }



        private void AddToOrder(Order currentOrder, XDocument doc)
        {
            //Order No
            currentOrder.OrderNumber = Convert.ToInt32(doc.XPathSelectElements("//Field[Name[.=\"Order No\"]]/Data").ToList()[0].Value);
            //Customer No.
            currentOrder.CustomerNumber = Convert.ToInt32(doc.XPathSelectElements("//Field[Name[.=\"Customer No\"]]/Data").ToList()[0].Value);
            //Customer Name
            currentOrder.CustomerName = doc.XPathSelectElements("//Field[Name[.=\"Customer Name\"]]/Data").ToList()[0].Value;
            //Document Type
            currentOrder.DocumentType = doc.XPathSelectElements("//Field[Name[.=\"Document Type\"]]/Data").ToList()[0].Value;
            //PO Number
            currentOrder.PONumber = Convert.ToInt32(doc.XPathSelectElements("//Field[Name[.=\"PO Number\"]]/Data").ToList()[0].Value);
            //Order Date
            currentOrder.OrderDate = Convert.ToDateTime(doc.XPathSelectElements("//Field[Name[.=\"Order Date\"]]/Data").ToList()[0].Value);
            //Document Type
            currentOrder.CreditStatus = doc.XPathSelectElements("//Field[Name[.=\"CreditStatus\"]]/Data").ToList()[0].Value;
            //wsf_inbox
            currentOrder.WSF_Inbox = doc.XPathSelectElements("//Field[Name[.=\"wsf_inbox\"]]/Data").ToList()[0].Value;
            //order status
            currentOrder.StatusId = 1; //Pending Approval.

            currentOrder.CreatedDate = DateTime.Now;
            currentOrder.ModifiedDate = DateTime.Now;

            db.Orders.Add(currentOrder);
        }

        private void AddFileToOrder(Order currentOrder, OrderFile currentFile, XDocument doc, string xmlFileName)
        {
            
            currentFile.FilePath = "/InProgress"; //this.orderInboxPath;
            //currentFile.Orders = currentOrder;
            currentFile.OrderNumber = currentOrder.OrderNumber;

            //Document Type
            currentFile.Description = doc.XPathSelectElements("//Field[Name[.=\"Document Type\"]]/Data").ToList()[0].Value;
            currentFile.FileName = doc.XPathSelectElements("//Filespec").ToList()[0].Value;
            currentFile.Version = 1;

            currentFile.XMLFilePath = xmlFileName;

            //determine file type
            var splitName = currentFile.FileName.Split('.');
            if (splitName[splitName.Length - 1].ToUpper() == "PDF")
            {
                currentFile.DocumentType = "PDF";
            }
            else
            if (splitName[splitName.Length - 1].ToUpper() == "MSG")
            {
                currentFile.DocumentType = "Email";
            }
            else
            {
                currentFile.DocumentType = "Misc. Document";
            }


            db.OrderFiles.Add(currentFile);
        }

        private void WriteToLog(string message)
        {
            //log to event viewer.
            if (!EventLog.SourceExists("Chauvet Order Approval Manager"))
            {
                EventLog.CreateEventSource("Chauvet Order Approval Manager", "Application");
            }

            //Create an EventLog instance and assign its source.
            EventLog eventLog = new EventLog();
            eventLog.Source = "Chauvet Order Approval Manager";

            //Write an informational entry to the event log.
            eventLog.WriteEntry("Error: " + message, EventLogEntryType.Error);
        }

        private void fswOrderInbox_Changed(object sender, FileSystemEventArgs e)
        {

        }
    }
}
