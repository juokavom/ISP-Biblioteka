using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISP_Biblioteka.Controllers
{
    public class BooksController : Controller
    {
        // GET: Book
        public ActionResult Index()
        {

            return View(Models.Books.getBooks());
        }
        public ActionResult Search()
        {
            return View(Models.Books.getBooks());
        }
        public ActionResult Tvarkyti()
        {
            return View();
        }
        public ActionResult History()
        {
            return View();
        }

       
    }
}