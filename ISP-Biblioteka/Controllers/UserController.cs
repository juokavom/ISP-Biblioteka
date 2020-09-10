using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP_Biblioteka.Models;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace ISP_Biblioteka.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            /*List<User> list1 = new List<User>();
            string mainconn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
            MySqlConnection mysql = new MySqlConnection(mainconn);
            string query = "select * from test";
            MySqlCommand comm = new MySqlCommand(query);
            comm.Connection = mysql;
            mysql.Open();
            MySqlDataReader dr = comm.ExecuteReader();
            while (dr.Read())
            {
                list1.Add(new Models.User 
                {
                    Name = dr["name"].ToString(),
                    Id = Int32.Parse(dr["id"].ToString()),
                    ImagePath = "~/Image/User/" + dr["image"].ToString()
                });
            }
            mysql.Close();*/

           // return View(list1);
            return View();
        }
    }
}