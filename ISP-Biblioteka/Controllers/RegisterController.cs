using ISP_Biblioteka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace ISP_Biblioteka.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult AddUser(User user)
        {
            user.Validation = 0;
            user.Type = 1;
            string gender = "female";
            if (user.Gender == 1) gender = "male";
            user.Image = string.Format("~/Image/User/{0}.png", gender);

            BuildEmailTemplate(model.ID);
            return Json("Registration Successfull", JsonRequestBehavior.AllowGet);
        }
        
        public string CheckUser(User user)
        {
            string result = "Fail";
            if (!user.chechUser()) {
                result = "Success";
            }
            return result;
        }
    }
}