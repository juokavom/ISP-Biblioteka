using ISP_Biblioteka.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISP_Biblioteka.Controllers
{
    public class OrdersController : Controller
    {
        // GET: Orders
        public ActionResult Index(string email)
        {
            Models.User user = new Models.User { Email = email };
            user.updateValues();
            return View(user);
        }
        public ActionResult RequestBook()
        {
           
            return View();
        }
        public ActionResult MakeOrder(Order  order)
        {
            int id = order.ID;
            int bid = order.FK_book_id;
            return View();
        }
        public ActionResult SubmitOrder(int bID)
        {
            Order order = new Order { };
            order.FK_book_id = bID;
            order.createOrderRequest();
            return null;
        }
        public ActionResult OrderRequests()
        {
            return View();
        }
        [HttpPost]
        public ActionResult OrderRequests(Order order)
        {
            order.Validation_date = DateTime.Now;
            order.Return_date = DateTime.Now.AddDays(14);
            order.Borrow_date = DateTime.Now.AddDays(1);
            order.validateOrder();
            return View(order);
        }
    }
}