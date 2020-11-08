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
    public class Book
    {

        public int id { get; set; }
        public string Title { get; set; }
        public DateTime Year { get; set; }
        public string Description { get; set; }
        //Unikalus lenteleje
        public int Pages { get; set; }
        public string ISBN { get; set; }
        public DateTime Creation_date { get; set; }
        public int Price { get; set; }
        //Nuotraukos kelias
        public string Image { get; set; }
       
        public Exception insertToDb()
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = @"INSERT INTO `book`(`id`, `title`, `year`, `description`, `pages`, `ISBN` , `creation_date`, `price`, `image`) " +
                       "VALUES(null,?title,?year,?description,?pages,?ISBN,?creation_date,?price,?image);";

                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?title", MySqlDbType.VarChar).Value = Title;
                mySqlCommand.Parameters.Add("?year", MySqlDbType.VarChar).Value = Year;
                mySqlCommand.Parameters.Add("?description", MySqlDbType.VarChar).Value = Description;
                mySqlCommand.Parameters.Add("?pages", MySqlDbType.VarChar).Value = Pages;
                mySqlCommand.Parameters.Add("?ISBN", MySqlDbType.VarChar).Value = ISBN;
                mySqlCommand.Parameters.Add("?creation_date", MySqlDbType.VarChar).Value = Creation_date;
                mySqlCommand.Parameters.Add("?price", MySqlDbType.VarChar).Value = Price;
                mySqlCommand.Parameters.Add("?image", MySqlDbType.Int32).Value = Image;
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

        public Exception updateValues()
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = @"SELECT * FROM `book` WHERE `id` = ?id";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?id", MySqlDbType.VarChar).Value = Title;
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                DataTable dt = new DataTable();
                mda.Fill(dt);
                mySqlConnection.Close();

                if (dt.Rows.Count == 1)
                {
                    id = Convert.ToInt16(dt.Rows[0]["id"]);
                    Title = Convert.ToString(dt.Rows[0]["title"]);
                    Year = Convert.ToDateTime(dt.Rows[0]["year"]);
                    Description = Convert.ToString(dt.Rows[0]["description"]);
                    Pages = Convert.ToInt16(dt.Rows[0]["pages"]);
                    ISBN = Convert.ToString(dt.Rows[0]["ISBN"]);
                    Creation_date = Convert.ToDateTime(dt.Rows[0]["creation_date"]);
                    Price = Convert.ToInt16(dt.Rows[0]["price"]);
                    Image = Convert.ToString(dt.Rows[0]["image"]);
                    return null;
                }
                else if (dt.Rows.Count > 1)
                {
                    throw new Exception("DB kelios knygos su tokiu id");
                }
                throw new Exception("DB nera knygos su tokiu id");
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return e;
            }
        }

       

   



        private PropertyInfo[] _PropertyInfos = null;
        public override string ToString()
        {
            if (_PropertyInfos == null)
                _PropertyInfos = this.GetType().GetProperties();

            var sb = new System.Text.StringBuilder();

            foreach (var info in _PropertyInfos)
            {
                var value = info.GetValue(this, null) ?? "(null)";
                sb.AppendLine(info.Name + ": " + value.ToString());
            }

            return sb.ToString();
        }
    }
}