using ISP_Biblioteka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
            List<User> usr = Models.Newsletter.getUsers();
            if (letter.Content != null && letter.Subject != null)
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("biblioteka389@gmail.com");
                foreach (var User in usr)
                {
                    mail.To.Add(new MailAddress(User.Email));
                    mail.Subject = letter.Subject;
                    mail.Body = letter.Content;
                }
                mail.IsBodyHtml = true;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("biblioteka389@gmail.com", "Visaigeras5");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                ViewBag.Message = "Išsiųsta!";
            }
            return View();
        }
    }
}