using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP_Biblioteka.Repos;
using ISP_Biblioteka.ViewModels;
using System.Net.Mail;
using System.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;

namespace ISP_Biblioteka.Controllers
{
    public class AtaskaitaController : Controller
    {
        AtaskaitaRepository repository = new AtaskaitaRepository();

        // GET: Ataskaita
        public ActionResult UzsakymuIstorija(int? period, int? user)
        {
            UzsakymuIstorijaViewModel2 uzsakymai = new UzsakymuIstorijaViewModel2();

            PopulateSelections3(uzsakymai);

            uzsakymai.period = period == null ? null : period;

            uzsakymai.user = user == null ? null : user;

            uzsakymai.uzsak = repository.getUzsakymuIstorija(uzsakymai.period, uzsakymai.user);

            return View(uzsakymai);
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
            string sub = "";
            
            if (siunt.ataskaitos_tipas == 1)
            {
                text = repository.getAtaskaitos();
                sub = "Neprisijungę vartotojai per 6 mėnesius";
            }
            else if(siunt.ataskaitos_tipas == 2)
            {
                text = repository.getAtaskaitos2();
                sub = "Knygų užsakymai (1 mėnesio)";
            }
                

            if (ModelState.IsValid & siunt.to != null)
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("biblioteka389@gmail.com");
                mail.To.Add(siunt.to);
                mail.Subject = sub;
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
        public ActionResult IsiskolineSkaitytojai(int? period)
        {
            IsiskolineViewModel2 isiskoline = new IsiskolineViewModel2();

            PopulateSelections5(isiskoline);

            isiskoline.period = period == null ? null : period;

            isiskoline.isiskol = repository.getIsiskoline(isiskoline.period);

            return View(isiskoline);
        }
        public ActionResult KnyguStatistika(DateTime? year_from, DateTime? year_to)
        {
            KnyguStatistikaViewModel2 knygos = new KnyguStatistikaViewModel2();

            knygos.year_from = year_from == null ? null : year_from;
            knygos.year_to = year_to == null ? null : year_to;

            knygos.knyg = repository.getKnyguStatistika(knygos.year_from, knygos.year_to);

            return View(knygos);
        }
        public ActionResult MetMenAtaskaita()
        {
            MetMenAtaskaitaViewModel2 metMenAtask = new MetMenAtaskaitaViewModel2();

            PopulateSelections4(metMenAtask);

            metMenAtask.uzsak = repository.getMetMenAtaskaita(null);

            return View(metMenAtask);
        }

        [HttpPost]
        [ValidateInput(false)]
        public FileResult MetMenAtaskaita(string ExportData, int? period)
        {
            MetMenAtaskaitaViewModel2 metMenAtask = new MetMenAtaskaitaViewModel2();

            PopulateSelections4(metMenAtask);

            metMenAtask.period = period == null ? null : period;

            metMenAtask.uzsak = repository.getMetMenAtaskaita(metMenAtask.period);

            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                PdfPTable table = new PdfPTable(4);
                StringReader reader = new StringReader(ExportData);
                Document PdfFile = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(PdfFile, stream);
                if(metMenAtask.period == 1)
                {
                    PdfPCell cell = new PdfPCell(new Phrase("Menesine knygu pasiemimo ataskaita"));
                    cell.Colspan = 4;
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                else 
                {
                    PdfPCell cell = new PdfPCell(new Phrase("Metine knygu pasiemimo ataskaita"));
                    cell.Colspan = 4;
                    cell.HorizontalAlignment = 1;
                    table.AddCell(cell);
                }
                PdfFile.Open();

                table.AddCell(Convert.ToString("Vartotojas"));
                table.AddCell(Convert.ToString("Knyga"));
                table.AddCell(Convert.ToString("Pasiskolinimo data"));
                table.AddCell(Convert.ToString("Grazinti iki"));

                foreach (var i in metMenAtask.uzsak)
                {
                    table.AddCell(Convert.ToString(i.user));
                    table.AddCell(Convert.ToString(i.book));
                    table.AddCell(i.borrow_date.ToString("dd/MM/yyyy"));
                    table.AddCell(i.return_date.ToString("dd/MM/yyyy"));
                }
                PdfFile.Add(table);
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, PdfFile, reader);
                PdfFile.Close();
                return File(stream.ToArray(), "application/pdf", "ExportData.pdf");
            }
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
            selectListAtaskTipas.Add(new SelectListItem() { Value = Convert.ToString(2), Text = "Knygų užsakymai(1 mėnuo)" });

            foreach (var em in email)
            {
                selectListEmail.Add(new SelectListItem() { Value = Convert.ToString(em), Text = em.ToString() });
            }

            siunt.AtaskaitosList = selectListAtaskTipas;
            siunt.EmailList = selectListEmail;

        }

        public void PopulateSelections3(UzsakymuIstorijaViewModel2 vart)
        {
            List<SelectListItem> selectListlaikotarpiai = new List<SelectListItem>();
            List<SelectListItem> selectListUsers = new List<SelectListItem>();
            var users = repository.getUsers();


            selectListlaikotarpiai.Add(new SelectListItem() { Value = Convert.ToString(1), Text = "1 mėnuo" });
            selectListlaikotarpiai.Add(new SelectListItem() { Value = Convert.ToString(3), Text = "3 mėnesiai" });
            selectListlaikotarpiai.Add(new SelectListItem() { Value = Convert.ToString(6), Text = "6 mėnesiai" });
            selectListlaikotarpiai.Add(new SelectListItem() { Value = Convert.ToString(12), Text = "12 mėnesių" });

            foreach (var em in users)
            {
                selectListUsers.Add(new SelectListItem() { Value = Convert.ToString(em.Key), Text = em.Value.ToString() });
            }

            vart.LaikotarpisList = selectListlaikotarpiai;
            vart.VartotojasList = selectListUsers;
        }

        public void PopulateSelections4(MetMenAtaskaitaViewModel2 vart)
        {
            List<SelectListItem> selectListlaikotarpiai = new List<SelectListItem>();

            selectListlaikotarpiai.Add(new SelectListItem() { Value = Convert.ToString(1), Text = "Mėnesio" });
            selectListlaikotarpiai.Add(new SelectListItem() { Value = Convert.ToString(12), Text = "Metų" });

            vart.LaikotarpisList = selectListlaikotarpiai;

        }

        public void PopulateSelections5(IsiskolineViewModel2 isisk)
        {
            List<SelectListItem> selectListlaikotarpiai = new List<SelectListItem>();

            selectListlaikotarpiai.Add(new SelectListItem() { Value = Convert.ToString(1), Text = "Iki 1 mėnesio" });
            selectListlaikotarpiai.Add(new SelectListItem() { Value = Convert.ToString(3), Text = "Iki 3 mėnesių" });
            selectListlaikotarpiai.Add(new SelectListItem() { Value = Convert.ToString(6), Text = "Iki 6 mėnesių" });
            selectListlaikotarpiai.Add(new SelectListItem() { Value = Convert.ToString(12), Text = "Iki 12 mėnesių" });

            isisk.LaikotarpisList = selectListlaikotarpiai;

        }
    }
}