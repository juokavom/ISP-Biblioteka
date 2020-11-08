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
    }
}