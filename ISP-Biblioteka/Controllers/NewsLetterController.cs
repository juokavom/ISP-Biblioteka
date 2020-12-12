using ISP_Biblioteka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISP_Biblioteka.Controllers
{
    public class NewsLetterController : Controller
    {
        // GET: NewsLetter
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Newsletter letter)
        {
            string subject = letter.Subject;
            string content = letter.Content;
            letter.createNewsletter();
            return View();
        }
    }
}