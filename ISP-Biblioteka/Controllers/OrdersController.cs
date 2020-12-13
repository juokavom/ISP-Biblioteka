using ISP_Biblioteka.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
            var x = Session["id"].ToString();
            dynamic model = new ExpandoObject();
            model.Order = Models.User.getOrders(x);
            model.Book = Order.getBooks();
            return View(model);
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
            dynamic model = new ExpandoObject();
            model.Order = Order.getOrders();
            model.Book = Order.getBooks();
            model.User = Order.getUsers();
            return View(model);
        }
        [HttpPost]
        public ActionResult OrderRequests(int ID)
        {
            Order order = new Order { };
            if (ID != null)
            {
                order.ID = ID;
                order.Validation_date = DateTime.Now;
                order.Return_date = DateTime.Now.AddDays(14);
                order.Borrow_date = DateTime.Now.AddDays(1);
                order.validateOrder();
            }
            else
            {
                order.Validation_date = DateTime.MinValue;
                order.Return_date = DateTime.MinValue;
                order.Borrow_date = DateTime.MinValue;
                order.validateOrder();

            }
            dynamic model = new ExpandoObject();
            model.Order = Order.getOrders();
            model.Book = Order.getBooks();
            model.User = Order.getUsers();
            return View(model);
        }
    }
}