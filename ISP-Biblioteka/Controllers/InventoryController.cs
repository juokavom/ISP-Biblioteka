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
    public class InventoryController : Controller
    {
        // GET: Inventory
        public ActionResult Index()
        {
            ViewData["Inventory"] = GetAllInventory();
            ViewData["Locations"] = GetAllLocations();
            return View();
        }
        [HttpPost]
        public ActionResult Index(string searchName)
        {
            if(searchName == "")
            {
                ViewData["Inventory"] = GetAllInventory();
                ViewData["Locations"] = GetAllLocations();
                return View();
            }
            List<Inventory> allInventory = GetAllInventory();
            allInventory = allInventory.Where(inv => inv.name.ToLower().Contains(searchName.ToLower())).ToList<Inventory>();

            /*MySqlConnection mySQL = new MySqlConnection(ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString);

            string query = @"SELECT * FROM `inventory` WHERE `name` like %?searchName%";
            MySqlCommand sqlQuery = new MySqlCommand(query, mySQL);

            sqlQuery.Parameters.Add("?searchName", MySqlDbType.VarChar).Value = searchName;

            mySQL.Open();
            sqlQuery.ExecuteNonQuery();
            MySqlDataAdapter fetch = new MySqlDataAdapter(sqlQuery);

            DataTable table = new DataTable();
            fetch.Fill(table);
            mySQL.Close();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                allInventory.Add(new Inventory(table.Rows[i]));
            }*/

            ViewData["Inventory"] = allInventory;
            ViewData["Locations"] = GetAllLocations();
            return View();
        }

        public ActionResult Edit(int id)
        {
            Inventory inv = FindInventory(id);
            return View(inv);
        }

        public ActionResult WriteOff(int id)
        {
            Inventory inv = FindInventory(id);
            return View(inv);
        }

        [HttpPost]
        public ActionResult UpdateInventory(Inventory inv)
        {
            Debug.Write(inv);
            inv.UpdateDatabase();
            ViewData["Inventory"] = GetAllInventory();
            ViewData["Locations"] = GetAllLocations();

            return View("Index");
        }

        [HttpPost]
        public ActionResult UpdateWriteOff(Inventory model, int writeOffCount, string comment)
        {
            //Inventory inv = FindInventory(invID);
            Debug.Write(model);
            Debug.Write(comment);
            Debug.Write(writeOffCount);
            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString);
            string query = "";

            if (writeOffCount == model.count)
            {
                query = "DELETE FROM `inventory` WHERE `id`=?id";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Add("?id", MySqlDbType.Int32).Value = model.id;
                connection.Open();
                command.ExecuteNonQuery();


            }
            else
            {
                model.count -= writeOffCount;
                model.UpdateDatabase();

                query = "INSERT INTO `writeoff`(`itemID`, `amount`, `date`, `comment`) VALUES " +
                    "(?itemID, ?amount, CURRENT_DATE, ?comment)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Add("?itemID", MySqlDbType.Int32).Value = model.id;
                command.Parameters.Add("?amount", MySqlDbType.Int32).Value = writeOffCount;
                command.Parameters.Add("?comment", MySqlDbType.Text).Value = comment;
                connection.Open();
                command.ExecuteNonQuery();
            }

            ViewData["Inventory"] = GetAllInventory();
            ViewData["Locations"] = GetAllLocations();

            return View("Index");
        }

        [HttpPost]
        public ActionResult AddInventory(string name, decimal cost, int count, string type, DateTime expirationDate, string loc)
        {
            /*Inventory.InventoryTypes t;
            Enum.TryParse(inventoryType.ToString(), out t);*/

            Inventory inv = new Inventory(name, type, count, cost, expirationDate, int.Parse(loc));

            inv.InsertToDatabase();
            return RedirectToAction("Index", "Inventory");
        }

        public List<Inventory> GetAllInventory()
        {
            List<Inventory> allInventory = new List<Inventory>();

            MySqlConnection mySQL = new MySqlConnection(ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString);
            string query = @"SELECT * FROM `inventory`";

            MySqlCommand sqlQuery = new MySqlCommand(query, mySQL);

            mySQL.Open();
            sqlQuery.ExecuteNonQuery();
            MySqlDataAdapter fetch = new MySqlDataAdapter(sqlQuery);

            DataTable table = new DataTable();
            fetch.Fill(table);
            mySQL.Close();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                allInventory.Add(new Inventory(table.Rows[i]));
            }

            return allInventory;
        }

        public List<Location> GetAllLocations()
        {
            List<Location> locations = new List<Location>();

            MySqlConnection mySQL = new MySqlConnection(ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString);
            string query = @"SELECT * FROM `location`";

            MySqlCommand sqlQuery = new MySqlCommand(query, mySQL);

            mySQL.Open();
            sqlQuery.ExecuteNonQuery();
            MySqlDataAdapter fetch = new MySqlDataAdapter(sqlQuery);

            DataTable table = new DataTable();
            fetch.Fill(table);
            mySQL.Close();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                locations.Add(new Location(table.Rows[i]));
            }

            return locations;
        }

        

        Inventory FindInventory(int id)
        {
            MySqlConnection mySQL = new MySqlConnection(ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString);

            string query = @"SELECT * FROM `inventory` WHERE `id` = ?id";
            MySqlCommand sqlQuery = new MySqlCommand(query, mySQL);

            sqlQuery.Parameters.Add("?id", MySqlDbType.Int16).Value = id;

            mySQL.Open();
            sqlQuery.ExecuteNonQuery();
            MySqlDataAdapter fetch = new MySqlDataAdapter(sqlQuery);

            DataTable table = new DataTable();
            fetch.Fill(table);
            mySQL.Close();

            return new Inventory(table.Rows[0]);
        }

    }
}