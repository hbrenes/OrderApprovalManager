using OAM.Models;
using OAMDataAccess.Repository;
using OAMDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OAM.Controllers
{
    public class OrdersController : Controller
    {

        OrdersRepository dbOrders = new OrdersRepository();
        SettingsRepository dbSettings = new SettingsRepository();
        OrderFilesRepository dbFiles = new OrderFilesRepository();
        //
        // GET: /Orders/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail(int id)
        {
            var res = dbOrders.GetOrderByOrderNo(id);

            List<OAMDataAccess.OrderHistory> hist = new List<OrderHistory>();
            foreach (var item in res.OrderHistory)
	        {
                
	        }

            OrderViewModel order = new OrderViewModel()
            {
                OrderId = res.OrderId,
                AssignedTo = res.AssignedTo,
                CustomerName = res.CustomerName,
                CustomerNumber = res.CustomerNumber,
                OrderDate = res.OrderDate,
                OrderNumber = res.OrderNumber,
                StatusId = res.StatusId,
                PoNumber = res.PONumber,
                Comment = res.Comment,
                Files = dbOrders.GetFilesFromOrder(id),
                History = dbOrders.GetHistory(res.OrderId),
                Comments = dbOrders.GetOrderCommentsByOrderId(res.OrderId)

            };

            return View(order);
        }

        [HttpPost]
        public JsonResult Update(OrderViewModel vm)
        {
            dbOrders.UpdatePartial(vm.OrderNumber,vm.StatusId, vm.AssignedTo, vm.Comment, vm.AssignedTo);

            //check if status is approved
            if (vm.StatusId == 2)
            {
                //move files to
                //get files to move
                var files = dbOrders.GetFilesFromOrder(vm.OrderNumber);

                int orderId = dbOrders.GetOrderIdByOrderNumber(vm.OrderNumber);

                string sourcePath = dbSettings.GetById("OrderInProgressPath").Value;

                string destinationPath = dbSettings.GetById("OrderApprovedPath").Value;


                foreach (var file in files)
	            {
                    //TODO: xmlFilePath as paratemer
                    FileMove(sourcePath, destinationPath, file.FileName, file.XMLFilePath);
                    //update path in db
                    file.FilePath = "/Approved";
                    dbFiles.UpdateFile(file);

	            }

                //add history order Approved
                OrderHistory h = new OrderHistory();
                h.ActionBy = User.Identity.Name.Split('\\')[1]; //logged user
                h.ActionDate = DateTime.Now;
                h.ActionName = "Status Change";
                h.ActionValue = String.Format("Order approved by {0}.", h.ActionBy);
                h.OrderId = orderId;
                h.OrderStatusId = vm.StatusId;

                dbOrders.AddHistory(h);
                
            }


            //check if status is abandoned
            if (vm.StatusId == 5)
            {
                var files = dbOrders.GetFilesFromOrder(vm.OrderNumber);

                int orderId = dbOrders.GetOrderIdByOrderNumber(vm.OrderNumber);

                string sourcePath = dbSettings.GetById("OrderInProgressPath").Value;

                string destinationPath = dbSettings.GetById("OrderAbandonedPath").Value;


                foreach (var file in files)
                {
                    //TODO: xmlFilePath as paratemer
                    FileMove(sourcePath, destinationPath, file.FileName, file.XMLFilePath);
                    
                    //update path in db
                    file.FilePath = "/Abandoned";
                    dbFiles.UpdateFile(file);
                }

                //add history order Abandoned
                OrderHistory h = new OrderHistory();
                h.ActionBy = User.Identity.Name.Split('\\')[1]; //logged user
                h.ActionDate = DateTime.Now;
                h.ActionName = "Status Change";
                h.ActionValue = String.Format("Order abandoned by {0}.", h.ActionBy);
                h.OrderId = orderId;
                h.OrderStatusId = vm.StatusId;

                dbOrders.AddHistory(h);
            }

            if (vm.StatusId == 1 || vm.StatusId == 3 || vm.StatusId == 4)
            {
                int orderId = dbOrders.GetOrderIdByOrderNumber(vm.OrderNumber);

                //add history order Abandoned
                OrderHistory h = new OrderHistory();
                h.ActionBy = User.Identity.Name.Split('\\')[1]; //logged user
                h.ActionDate = DateTime.Now;
                h.ActionName = "Status Change";
                h.ActionValue = String.Format("Order status changed by {0}.", h.ActionBy);
                h.OrderId = orderId;
                h.OrderStatusId = vm.StatusId;

                dbOrders.AddHistory(h);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Assign(OrdersViewModel vm)
        {
            foreach (var order in vm.Orders)
            {
                if (order.Selected)
                {
                    dbOrders.AssignOrder(order.OrderNumber, order.AssignedTo);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddComment(CommentViewModel vm)
        {
            OrderComments c = new OrderComments();
            c.OrderId = vm.OrderId;
            c.OrderCommentDate = DateTime.Now;
            c.OrderComment = vm.OrderComment;
            c.OrderCommentUser = User.Identity.Name.Split('\\')[1]; //logged user
            dbOrders.AddComment(c);
           
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //TODO: xmlFilePath as paratemer
        private void FileMove(string source, string destination, string fileName, string xmlFileName)
        {
            //move file

            if (System.IO.File.Exists(destination + "\\" + fileName))
            {
                System.IO.File.Delete(destination + "\\" + fileName);
            }

            System.IO.File.Move(source + "\\" + fileName, destination + "\\" + fileName);


            //move xml file
            if (System.IO.File.Exists(destination + "\\" + xmlFileName))
            {
                System.IO.File.Delete(destination + "\\" + xmlFileName);
            }

            System.IO.File.Move(source + "\\" + xmlFileName, destination + "\\" + xmlFileName);
        }
        
	}
}