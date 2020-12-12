using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ISP_Biblioteka.ViewModels
{
    public class MetMenAtaskaitaViewModel2
    {
        public List<MetMenAtaskaitaViewModel1> uzsak { get; set; }

        [DisplayName("Laikotarpis: ")]
        public int? period { get; set; }
        public IList<SelectListItem> LaikotarpisList { get; set; }
    }
}