using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISP_Biblioteka.ViewModels
{
    public class AtaskaituSiuntimasViewModel
    {
        public int? ataskaitos_tipas { get; set; }
        public string from { get; set; }

        public string to { get; set; }

        public string subject { get; set; }

        public string body { get; set; }
        public IList<SelectListItem> AtaskaitosList { get; set; }
        public IList<SelectListItem> EmailList { get; set; }
    }
}