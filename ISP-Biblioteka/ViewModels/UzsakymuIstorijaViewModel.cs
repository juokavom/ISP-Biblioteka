using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISP_Biblioteka.ViewModels
{
    public class UzsakymuIstorijaViewModel
    {
        public decimal visoSumaSutartciu { get; set; }
        public decimal visoSumaPaslauga { get; set; }

        public decimal sumaIsViso { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Data nuo: ")]
        public DateTime? nuo { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Data iki: ")]
        public DateTime? iki { get; set; }
        [DisplayName("Darbuotojas: ")]
        public string darbuotojas { get; set; }
        public IList<SelectListItem> DarbuotojaiList { get; set; }
        [DisplayName("Konvertuoti į: ")]
        public string valiuta { get; set; }
        public IList<SelectListItem> ValiutosList { get; set; }
        [DisplayName("Numeris: ")]
        public string numeris { get; set; }
        public IList<SelectListItem> NumeriaiList { get; set; }
    }
}