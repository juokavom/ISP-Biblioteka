using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISP_Biblioteka.ViewModels
{
    public class KnyguStatistikaViewModel1
    {
        [DisplayName("Pavadinimas")]
        public string title { get; set; }
        [DisplayName("Išleidimo metai ")]
        public DateTime year { get; set; }
        [DisplayName("Puslapiai ")]
        public int pages { get; set; }
        [DisplayName("ISBN ")]
        public string isbn { get; set; }
    }
}