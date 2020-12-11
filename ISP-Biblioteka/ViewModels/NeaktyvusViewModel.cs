using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISP_Biblioteka.ViewModels
{
    public class NeaktyvusViewModel
    {
        public List<NeaktyvusVartotojaiViewModel> neaktyvus { get; set; }

        [DisplayName("Neaktyvumo laikotarpis: ")]
        public int? period { get; set; }
        public IList<SelectListItem> LaikotarpisList { get; set; }
    }
}