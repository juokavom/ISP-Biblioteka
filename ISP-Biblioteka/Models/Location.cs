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
    public class Location
    {
        string zone { get; set; }
        public int? id { get; set; }
        int shelfNumber { get; set; }
        int shelfRow { get; set; }

        public Location(string zoneName, int shelfNumber, int shelfRow)
        {
            this.zone = zoneName;
            this.shelfNumber = shelfNumber;
            this.shelfRow = shelfRow;
        }

        public Location(DataRow row)
        {
            zone = Convert.ToString(row["zone"]);
            id = Convert.ToInt16(row["id"]);
            shelfNumber = Convert.ToInt16(row["shelf_number"]);
            shelfRow = Convert.ToInt16(row["shelf_row"]);
        }

        public Exception InsertToDatabase()
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string query = @"INSERT INTO `location`(`zone`, `shelf_number`, `shelf_row`) " +
                       "VALUES (?zone, ?shelfN, ?shelfR);";

                MySqlCommand mySqlCommand = new MySqlCommand(query, mySqlConnection);

                //mySqlCommand.Parameters.Add("?type", MySqlDbType.VarChar).Value = type.ToString();
                mySqlCommand.Parameters.Add("?zone", MySqlDbType.VarChar).Value = zone;
                mySqlCommand.Parameters.Add("?shelfN", MySqlDbType.Int16).Value = shelfNumber;
                mySqlCommand.Parameters.Add("?shelfR", MySqlDbType.Int16).Value = shelfRow;

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

        public override string ToString()
        {
            return string.Format($"Zona {zone} (ID {id}), lentyna {shelfNumber}, eilė {shelfRow}");
        }
    }
}