using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP_Biblioteka.Models;
using MySql.Data.MySqlClient;

namespace ISP_Biblioteka.Controllers
    {
        public class BookController : Controller
        {
            // GET: Book
            public ActionResult Index()
            {
                return View();
            }
        public ActionResult Search()
        {
            return View();
        }
  
        public ActionResult Edit(string title)
        {
            Models.Book book = new Models.Book { Title = title };
            book.updateValues();
            return NewMethod(book);
        }

        private ActionResult NewMethod(Book book)
        {
            return View(book);
        }

        /*      [HttpPost]
   [ValidateAntiForgeryToken]
 public ActionResult CreateBook(string title, string description, DateTime year, int pages, int price, string image, string ISBN)
   {
     //  Book book = new Book { Title = title, Description = description, Year = year, Pages = pages, Price = price, Image = image, ISBN = ISBN, Creation_date = DateTime.Now };
       try
       {
           string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
           MySqlConnection mySqlConnection = new MySqlConnection(conn);
           string sqlquery = @"INSERT INTO `book`(`id`, `title`, `year`, `description`, `pages`, `ISBN` , `creation_date`, `price`, `image`) " +
                  "VALUES(null,?title,?year,?description,?pages,?ISBN,?creation_date,?price,?image);";

           MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
           mySqlCommand.Parameters.Add("?title", MySqlDbType.VarChar).Value = title;
           mySqlCommand.Parameters.Add("?year", MySqlDbType.Date).Value = year;
           mySqlCommand.Parameters.Add("?description", MySqlDbType.VarChar).Value = description;
           mySqlCommand.Parameters.Add("?pages", MySqlDbType.Int32).Value = pages;
           mySqlCommand.Parameters.Add("?ISBN", MySqlDbType.VarChar).Value = ISBN;
           mySqlCommand.Parameters.Add("?creation_date", MySqlDbType.DateTime).Value = DateTime.Now;
           mySqlCommand.Parameters.Add("?price", MySqlDbType.Int32).Value = price;
           mySqlCommand.Parameters.Add("?image", MySqlDbType.Int32).Value = image;
           mySqlConnection.Open();
           mySqlCommand.ExecuteNonQuery();
           mySqlConnection.Close();
           return null;
       }
       catch (Exception)
       {

           throw;
       }
      //RedirectToAction("Index", "Books");
   }*/
        public ActionResult Create()
            {
                return View();
            }
            public ActionResult Remove(string title)
            {
                Models.Book book = new Models.Book { Title = title };
                book.updateValues();
                return View(book);
            }
            public JsonResult RemoveBook(string title)
            {
                Models.Book book = new Models.Book { Title = title };
                book.updateValues();
                Models.Book.removeBook(book.id);
                return Json("", JsonRequestBehavior.AllowGet);
            }
            public JsonResult RemoveBook2(int id)
            {
                Models.Book.removeBook(id);
                return Json("", JsonRequestBehavior.AllowGet);
            }

        public JsonResult EditBook(Models.Book book)
            {
                book.Image = string.Format("~/Image/Book/Pirma.png");
                book.updateToDb();
                return Json(JsonRequestBehavior.AllowGet);
            }
        public JsonResult CreateBook(Models.Book book)
        {
            book.Creation_date = DateTime.Now;
            book.Image = string.Format("~/Image/Book/Pirma.png");
            book.insertToDb();
            return Json(JsonRequestBehavior.AllowGet);
        }
    }
}
