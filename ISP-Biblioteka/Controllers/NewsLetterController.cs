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
    }
}