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
using System.Diagnostics;

namespace ISP_Biblioteka.Models
{
    public class Inventory
    {
        public enum InventoryTypes
        {
            Knyga,
            Kompiuteris,
            Monitorius,
            Spausdintuvas,
            Kita
        }

        public InventoryTypes type { get; set; }
        public int? id { get; set; }
        public int count { get; set; }
        public string name { get; set; }
        public decimal cost { get; set; }
        public DateTime registrationDate { get; set; }
        public DateTime expirationDate { get; set; }

        public int locationID;
        public string locationDescription { get; set; }

        public Inventory()
        {
            type = InventoryTypes.Kita;
            count = 0;
            name = "Klaida";
            cost = 0;
            registrationDate = new DateTime(2020, 1, 1);
            expirationDate = new DateTime(2020, 1, 1);
            locationID = 0;
        }

        public Inventory(string name, string type, int count, decimal cost, DateTime expirationDate, int locationID)
        {
            InventoryTypes t;
            Enum.TryParse(type, out t);
            this.type = t;
            //this.type = type;
            this.cost = cost;
            this.name = name;
            this.count = count;
            this.registrationDate = registrationDate;
            this.expirationDate = expirationDate;

            this.locationID = locationID;
            SetLocation();
        }

        public Inventory(DataRow data)
        {
            InventoryTypes t;
            Enum.TryParse(data["type"].ToString(), out t);
            type = t;

            id = Convert.ToInt32(data["id"]);
            name = Convert.ToString(data["name"]);
            cost = Convert.ToDecimal(data["cost"]);
            count = Convert.ToInt16(data["count"]);
            registrationDate = Convert.ToDateTime(data["registration_date"]);
            expirationDate = Convert.ToDateTime(data["end_date"]);
            locationID = Convert.ToInt16(data["location"]);

            SetLocation();
        }

        void SetLocation()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            string query = "SELECT * FROM `location` WHERE `id` = ?location";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.Add("?location", MySqlDbType.Int16).Value = locationID;

            connection.Open();
            command.ExecuteNonQuery();

            MySqlDataAdapter fetch = new MySqlDataAdapter(command);
            DataTable table = new DataTable();

            fetch.Fill(table);
            connection.Close();

            if (table.Rows.Count > 1)
                locationDescription = (new Location(table.Rows[0])).ToString();
            else if (table.Rows.Count == 0)
                locationDescription = "Dar nepadėta";
            else
                locationDescription = (new Location(table.Rows[0])).ToString(); 

        }

        public static Inventory Find(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
            MySqlConnection connection = new MySqlConnection(connectionString);

            string query = "SELECT * FROM `inventory` WHERE `id` = ?id";
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.Add("?id", MySqlDbType.Int16).Value = id;

            connection.Open();
            command.ExecuteNonQuery();

            MySqlDataAdapter fetch = new MySqlDataAdapter(command);
            DataTable table = new DataTable();

            fetch.Fill(table);
            connection.Close();

            if (table.Rows.Count > 1)
                return new Inventory("ID KARTOJASI", "Monitorius", 0, 0, new DateTime(2020, 01, 01), 0);
            else if(table.Rows.Count == 0)
                return new Inventory("NERASTA", "Monitorius", 0, 0, new DateTime(2020, 01, 01), 0);
            else return new Inventory(table.Rows[0]);

        }
        public Exception InsertToDatabase()
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string query = @"INSERT INTO `inventory`(`name`, `type`, `cost`, `registration_date`, `end_date`, `location`, `count`) " +
                       "VALUES (?name, ?type, ?cost, CURRENT_DATE, ?endDate, ?location, ?count);";

                MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection);

                //mySqlCommand.Parameters.Add("?type", MySqlDbType.VarChar).Value = type.ToString();
                mySqlCommand.Parameters.Add("?name", MySqlDbType.VarChar).Value = name;
                mySqlCommand.Parameters.Add("?type", MySqlDbType.VarChar).Value = type;
                mySqlCommand.Parameters.Add("?cost", MySqlDbType.Decimal).Value = cost;
                mySqlCommand.Parameters.Add("?count", MySqlDbType.Int16).Value = count;
                mySqlCommand.Parameters.Add("?endDate", MySqlDbType.Date).Value = expirationDate;
                mySqlCommand.Parameters.Add("?location", MySqlDbType.Int16).Value = locationID;

                Debug.WriteLine(mySqlCommand.ToString());

                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Debug.WriteLine(this);
                return e;
            }
        }

        public Exception UpdateDatabase()
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = @"UPDATE `inventory` SET `name`=?name, `count` = ?count, `type`=?type, `cost`=?cost, `registration_date`=?regDate, `end_date` =?endDate, `location`=?loc WHERE `id`=?id";

                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?id", MySqlDbType.Int32).Value = id;
                mySqlCommand.Parameters.Add("?loc", MySqlDbType.Int32).Value = locationID;
                mySqlCommand.Parameters.Add("?count", MySqlDbType.Int32).Value = count;
                mySqlCommand.Parameters.Add("?type", MySqlDbType.VarChar).Value = type.ToString();
                mySqlCommand.Parameters.Add("?name", MySqlDbType.VarChar).Value = name;
                mySqlCommand.Parameters.Add("?cost", MySqlDbType.Decimal).Value = cost;
                mySqlCommand.Parameters.Add("?regDate", MySqlDbType.Date).Value = registrationDate;
                mySqlCommand.Parameters.Add("?endDate", MySqlDbType.Date).Value = expirationDate;
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                mySqlConnection.Close();
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return e;
            }

        }


        public override string ToString()
        {
            return string.Format($"ID: {id} - pavadinimas - {name} - {type} - {cost} - registruota {registrationDate}, galioja iki {expirationDate}");
        }
    }


}