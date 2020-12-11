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
        public string? ataskaitos_tipas { get; set; }
        public IList<SelectListItem> AtaskaitosList { get; set; }
    }
}