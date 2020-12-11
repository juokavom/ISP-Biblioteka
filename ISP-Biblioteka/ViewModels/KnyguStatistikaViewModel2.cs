using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISP_Biblioteka.ViewModels
{
    public class KnyguStatistikaViewModel2
    {
        public List<KnyguStatistikaViewModel1> knyg { get; set; }

        [DisplayName("Išleista nuo: ")]
        public DateTime? year_from { get; set; }

        [DisplayName("Išleista iki: ")]
        public DateTime? year_to { get; set; }
    }
}