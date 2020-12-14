using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using ISP_Biblioteka.Models;
using ISP_Biblioteka.ViewModels;
using System.Diagnostics;


namespace ISP_Biblioteka.Controllers
{
    public class LocationController : Controller
    {
        [HttpPost]
        public ActionResult Add(string zoneName, int shelfNumber, int shelfRow)
        {
            Location loc = new Location(zoneName, shelfNumber, shelfRow);

            loc.InsertToDatabase();
            return RedirectToAction("Index", "Inventory");
        }
    }
}