using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISP_Biblioteka.ViewModels
{
    public class NeaktyvusVartotojaiViewModel
    {
        [DisplayName("Vardas")]
        public string name { get; set; }
        [DisplayName("Pavardė ")]
        public string surname { get; set; }
        [DisplayName("El. paštas ")]
        public string email { get; set; }
        [DisplayName("Paskutinis prisijungimas ")]
        public DateTime last_login { get; set; }
    }
}