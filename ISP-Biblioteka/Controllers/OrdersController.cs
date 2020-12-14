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
        public ActionResult Index()
        {
            var x = Session["id"].ToString();
            dynamic model = new ExpandoObject();
            model.Order = Models.User.getOrders(x);
            model.Book = Order.getBooks();
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(int ident)
        {
            Order order = new Order { };
            order.Validation_date = DateTime.MinValue.AddYears(2);
            order.Return_date = DateTime.MinValue;
            order.Borrow_date = DateTime.MinValue;
            order.ID = ident;
            order.FK_user_id = int.Parse(Session["id"].ToString());
            order.lostOrder();
            dynamic model = new ExpandoObject();
            model.Order = Models.User.getOrders(Session["id"].ToString());
            model.Book = Order.getBooks();
            return View(model);
        }
        public ActionResult RequestBook()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RequestBook(string name)
        {
            Book book = new Book { };
            book.Title = name;
            book.Creation_date = DateTime.MinValue;
            book.insertToDb();

            return View();
        }
        public ActionResult MakeOrder()
        {
            Order order = new Order { };
            List<Order> ord = Order.getOrders();
            int n = 0;
            int ords = 0;
            foreach (var Orde in ord)
            {
                if (Orde.FK_user_id == int.Parse(Session["id"].ToString()) && Orde.Validation_date == DateTime.MinValue.AddYears(2))
                {
                    n++;
                }
                if (Orde.FK_user_id == int.Parse(Session["id"].ToString()) && Orde.Return_date > DateTime.Now)
                {
                    ords++;
                }
            }
            if (n < 2)
            {
                if (ords < 4)
                {
                    order.FK_book_id = 9;
                    order.FK_user_id = int.Parse(Session["id"].ToString());
                    order.FK_rating_id = 0;
                    order.Validation_date = DateTime.MinValue.AddYears(1);
                    order.Return_date = DateTime.MinValue;
                    order.Borrow_date = DateTime.Now;
                    order.createOrderRequest();
                    ViewBag.Message = "Užsakymas atliktas! Palaukite kol bibliotekininkė jį patvirtins";
                }
                else
                {
                    ViewBag.Message = "Užsakymas nepavyko! Just turite perdaug aktyvių užsakymų";
                }
            }
            else
            {
                if (ords < 2)
                {
                    order.FK_book_id = 9;
                    order.FK_user_id = int.Parse(Session["id"].ToString());
                    order.FK_rating_id = 0;
                    order.Validation_date = DateTime.MinValue.AddYears(1);
                    order.Return_date = DateTime.MinValue;
                    order.Borrow_date = DateTime.Now;
                    order.createOrderRequest();
                    ViewBag.Message = "Užsakymas atliktas!";
                }
                else
                {
                    ViewBag.Message = "Užsakymas nepavyko! Just turite perdaug aktyvių užsakymų";
                }
            }
            
            dynamic model = new ExpandoObject();
            model.Order = Order.getOrders();
            model.Book = Order.getBooks();
            model.User = Order.getUsers();
            model.Author = Order.getAuthor();
            model.sesh = 0;
            return View(model);
        }

        public ActionResult OrderRequests()
        {
            dynamic model = new ExpandoObject();
            model.Order = Order.getOrders();
            model.Book = Order.getBooks();
            model.User = Order.getUsers();
            model.Author = Order.getAuthor();
            model.value = 5;
            return View(model);
        }
        [HttpPost]
        public ActionResult OrderRequests(string ID, string IDs)
        {
            Order order = new Order { };
            if (!string.IsNullOrEmpty(ID))
            {
                order.ID = int.Parse(ID);
                order.Validation_date = DateTime.Now;
                order.Return_date = DateTime.Now.AddDays(14);
                order.Borrow_date = DateTime.Now.AddDays(1);
                order.validateOrder();
            }
            else
            {
                order.ID = int.Parse(IDs);
                order.Validation_date = DateTime.MinValue;
                order.Return_date = DateTime.MinValue;
                order.Borrow_date = DateTime.MinValue;
                order.validateOrder();

            }
            dynamic model = new ExpandoObject();
            model.Order = Order.getOrders();
            model.Book = Order.getBooks();
            model.User = Order.getUsers();
            model.Author = Order.getAuthor();
            model.value = IDs;
            return View(model);
        }
    }
}