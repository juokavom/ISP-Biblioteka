using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISP_Biblioteka.Controllers
{
    public class AtaskaitaController : Controller
    {
        // GET: Ataskaita
        public ActionResult UzsakymuIstorija()
        {
            return View();
        }
        public ActionResult AtaskaituSiuntimas()
        {
            return View();
        }
        public ActionResult IsiskolineSkaitytojai()
        {
            return View();
        }
        public ActionResult KnyguStatistika()
        {
            return View();
        }
        public ActionResult MetMenAtaskaita()
        {
            return View();
        }
        public ActionResult NeaktyvusVartotojai()
        {
            return View();
        }
    }
}