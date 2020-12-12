using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISP_Biblioteka.ViewModels
{
    public class IsiskolineViewModel2
    {
        public List<IsiskolineViewModel1> isiskol { get; set; }

        [DisplayName("Vėluojama grąžini: ")]
        public int? period { get; set; }
        public IList<SelectListItem> LaikotarpisList { get; set; }
    }
}