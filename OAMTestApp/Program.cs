using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml.Linq;
using System.Xml.XPath;
using System.DirectoryServices.AccountManagement;

namespace OAMTestApp
{
    class Program
    {
        static void Main(string[] args)
        {

                    PrincipalContext ctx = new PrincipalContext(ContextType.Machine, "Brenes"); 
                    GroupPrincipal grp = GroupPrincipal.FindByIdentity(ctx, IdentityType.Name, "Administrators");

                    if (grp != null)
                    {
                        foreach (Principal p in grp.GetMembers(true))
                        {
                            Console.WriteLine(p.Name); 
                        }


                        grp.Dispose();
                        ctx.Dispose();
                    }
                    Console.ReadKey();

            //DataSet ds = new DataSet();

            //ds.ReadXml("XMLFile1.xml");

            //ds.WriteXml("OrderDoc.xml",XmlWriteMode.);

            //var doc = XDocument.Load("XMLFile1.xml");


            //var result = doc.XPathSelectElements("//GobyMonitor/Document/Fields/Field").ToList();

            

            //foreach (var item in result)
            //{
            //    var childs = item.Elements().ToList();

            //    string field = childs[0].Value;
            //    string data = childs[1].Value;
            //}

        }
    }
}
