using ISP_Biblioteka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISP_Biblioteka.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult SaveData(User model)
        {
            model.Validation = 0;
            


            //BuildEmailTemplate(model.ID);
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