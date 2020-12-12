using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP_Biblioteka.Models;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Reflection;
using Microsoft.Ajax.Utilities;

namespace ISP_Biblioteka.Models
{
    public class Order
    {
        public int ID { get; set; }
        public DateTime Borrow_date { get; set; }
        public DateTime Return_date { get; set; }
        public DateTime Validation_date { get; set; }

        public Order()
        {

        }

        public Order(DataRow row)
        {
            ID = Convert.ToInt16(row["id"]);
            Borrow_date = Convert.ToDateTime(row["borrow_date"]);
            Return_date = Convert.ToDateTime(row["return_date"]);
            Validation_date = Convert.ToDateTime(row["validation_date"]);
        }
    }
}