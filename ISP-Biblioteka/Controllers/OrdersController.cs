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
        public ActionResult RequestBook(string email)
        {
            Models.User user = new Models.User { Email = email };
            user.updateValues();
            return View(user);
        }
        public ActionResult MakeOrder()
        {
            return View();
        }
        public ActionResult OrderRequests()
        {
            return View();
        }
    }
}