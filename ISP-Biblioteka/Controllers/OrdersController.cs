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
        public ActionResult MakeOrder()
        {
            Models.Order order = new Models.Order { };
            order.FK_book_id = 2;
            order.createOrderRequest();
            return View(order);
        }
        public ActionResult OrderRequests()
        {
            Models.Order order = new Models.Order { };
            order.Validation_date = DateTime.Now;
            order.Return_date = DateTime.Now.AddDays(14);
            order.Borrow_date = DateTime.Now.AddDays(1);
            order.validateOrder();
            return View(order);
        }
    }
}