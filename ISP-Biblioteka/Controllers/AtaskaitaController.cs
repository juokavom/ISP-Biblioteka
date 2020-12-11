using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP_Biblioteka.Repos;
using ISP_Biblioteka.ViewModels;
using System.Net.Mail;
using System.Net;

namespace ISP_Biblioteka.Controllers
{
    public class AtaskaitaController : Controller
    {
        AtaskaitaRepository repository = new AtaskaitaRepository();

        // GET: Ataskaita
        public ActionResult UzsakymuIstorija()
        {
            return View();
        }
        public ActionResult AtaskaituSiuntimas()
        {
            AtaskaituSiuntimasViewModel siunt = new AtaskaituSiuntimasViewModel();
            PopulateSelections2(siunt);
            return View(siunt);
        }
        [HttpPost]
        public ActionResult AtaskaituSiuntimas(AtaskaituSiuntimasViewModel _objModelMail)
        {
            AtaskaituSiuntimasViewModel siunt = new AtaskaituSiuntimasViewModel();
            siunt.ataskaitos_tipas = _objModelMail.ataskaitos_tipas == null ? null : _objModelMail.ataskaitos_tipas;
            siunt.to = _objModelMail.to == null ? null : _objModelMail.to;
            PopulateSelections2(siunt);
            string text = "";
            if (siunt.ataskaitos_tipas == 1)
            {
                text = repository.getAtaskaitos();
            }
                

            if (ModelState.IsValid & siunt.to != null)
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("biblioteka389@gmail.com");
                mail.To.Add(siunt.to);
                mail.Subject = "test ";
                mail.Body += text;

                mail.IsBodyHtml = true;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("biblioteka389@gmail.com", "Visaigeras5");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                ViewBag.Message = "Ataskaita išsiųsta!";
            }
            else
            {
                return View();
            }



            return View(siunt);
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
        public ActionResult NeaktyvusVartotojai(int? period)
        {
            NeaktyvusViewModel neaktyvus = new NeaktyvusViewModel();

            PopulateSelections1(neaktyvus);

            neaktyvus.period = period == null ? null : period;

            neaktyvus.neaktyvus = repository.getNeaktyvusVartotojai(neaktyvus.period);

            return View(neaktyvus);
        }

        public void PopulateSelections1(NeaktyvusViewModel vart)
        {
            List<SelectListItem> selectListlaikotarpiai = new List<SelectListItem>();

            selectListlaikotarpiai.Add(new SelectListItem() { Value = Convert.ToString(1), Text = "1 mėnuo" });
            selectListlaikotarpiai.Add(new SelectListItem() { Value = Convert.ToString(3), Text = "3 mėnesiai" });
            selectListlaikotarpiai.Add(new SelectListItem() { Value = Convert.ToString(6), Text = "6 mėnesiai" });
            selectListlaikotarpiai.Add(new SelectListItem() { Value = Convert.ToString(12), Text = "12 mėnesių" });

            vart.LaikotarpisList = selectListlaikotarpiai;

        }

        public void PopulateSelections2(AtaskaituSiuntimasViewModel siunt)
        {
            List<SelectListItem> selectListAtaskTipas = new List<SelectListItem>();
            List<SelectListItem> selectListEmail = new List<SelectListItem>();
            var email = repository.getEmail();

            selectListAtaskTipas.Add(new SelectListItem() { Value = Convert.ToString(1), Text = "Neaktyvūs vartotojai (6 mėnesiai)", Selected = true });
            selectListAtaskTipas.Add(new SelectListItem() { Value = Convert.ToString(3), Text = "test" });
            selectListAtaskTipas.Add(new SelectListItem() { Value = Convert.ToString(6), Text = "test" });
            selectListAtaskTipas.Add(new SelectListItem() { Value = Convert.ToString(12), Text = "test" });

            foreach (var em in email)
            {
                selectListEmail.Add(new SelectListItem() { Value = Convert.ToString(em), Text = em.ToString() });
            }

            siunt.AtaskaitosList = selectListAtaskTipas;
            siunt.EmailList = selectListEmail;

        }
    }
}