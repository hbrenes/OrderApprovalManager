using Newtonsoft.Json;
using OAM.Models;
using OAMDataAccess;
using OAMDataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OAM.Controllers
{
    public class HomeController : Controller
    {
        OrdersRepository dbOrders = new OrdersRepository();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult GetOrders()
        {
            var result = dbOrders.GetOrdersAll();
            JsonResult r = Json(result, JsonRequestBehavior.AllowGet);
            //JsonResult r = new JsonResult()
            //{
            //    Data = JsonConvert.SerializeObject(result),
            //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //}; //Json(result, JsonRequestBehavior.AllowGet);
            return r; 
        }

        public JsonResult GetOrdersByOrderNo(int orderNo)
        {
            //string p = Server.MapPath("~/App_Data/Files/InProgress");
            //string path = AppDomain.CurrentDomain.BaseDirectory + "uploads/";

            var result = dbOrders.GetOrdersByOrderNo(orderNo);

            OrdersViewModel orders = new OrdersViewModel();
            orders.Orders = result;

            JsonResult r = Json(orders, JsonRequestBehavior.AllowGet);
            //JsonResult r = new JsonResult()
            //{
            //    Data = JsonConvert.SerializeObject(result),
            //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //}; //Json(result, JsonRequestBehavior.AllowGet);
            return r;
        }

        public string GetUserName()
        {
            return User.Identity.Name.Split('\\')[1];
        }

        public JsonResult GetAllUsers()
        {
            var result = Helper.GetUsersOfGroup(System.Configuration.ConfigurationManager.AppSettings["UserGroup"]);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetOrdersByStatus(int statusId, string user, bool byUser)
        {
            var result = dbOrders.GetOrdersByStatus(statusId, user, byUser);

            OrdersViewModel orders = new OrdersViewModel();
            orders.Orders = result;
            
            JsonResult r = Json(orders, JsonRequestBehavior.AllowGet);
            //JsonResult r = new JsonResult()
            //{
            //    Data = JsonConvert.SerializeObject(result),
            //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //}; //Json(result, JsonRequestBehavior.AllowGet);
            return r;
        }

        public JsonResult GetOrdersByStatusAndDate(int statusId, string searchFrom, string searchTo, string user, bool byUser)
        {
            var result = dbOrders.GetOrdersByStatusAndDate(statusId, searchFrom, searchTo, user, byUser);

            OrdersViewModel orders = new OrdersViewModel();
            orders.Orders = result;

            JsonResult r = Json(orders, JsonRequestBehavior.AllowGet);
            //JsonResult r = new JsonResult()
            //{
            //    Data = JsonConvert.SerializeObject(result),
            //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //}; //Json(result, JsonRequestBehavior.AllowGet);
            return r;
        }
    }
}