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
        public int Pages { get; set; }
        public string ISBN { get; set; }
        public DateTime Creation_date { get; set; }
        public int Price { get; set; }
        //Nuotraukos kelias
        public string Image { get; set; }
        public Book()
        {

        }

        public Book(DataRow row)
        {
            id = Convert.ToInt16(row["id"]);
            Title = Convert.ToString(row["title"]);
            Year = Convert.ToDateTime(row["year"]);
            Description = Convert.ToString(row["description"]);
            Pages = Convert.ToInt16(row["pages"]);
            ISBN = Convert.ToString(row["ISBN"]);
            Creation_date = Convert.ToDateTime(row["creation_date"]);
            Price = Convert.ToInt16(row["price"]);
            Image = Convert.ToString(row["image"]);
        }

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
                mySqlCommand.Parameters.Add("?year", MySqlDbType.DateTime).Value = Year;
                mySqlCommand.Parameters.Add("?description", MySqlDbType.VarChar).Value = Description;
                mySqlCommand.Parameters.Add("?pages", MySqlDbType.Int32).Value = Pages;
                mySqlCommand.Parameters.Add("?ISBN", MySqlDbType.VarChar).Value = ISBN;
                mySqlCommand.Parameters.Add("?creation_date", MySqlDbType.DateTime).Value = Creation_date;
                mySqlCommand.Parameters.Add("?price", MySqlDbType.Int32).Value = Price;
                mySqlCommand.Parameters.Add("?image", MySqlDbType.VarChar).Value = Image;
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
                string sqlquery = @"SELECT * FROM `book` WHERE `title` = ?title";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?title", MySqlDbType.VarChar).Value = Title;
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
        public static Exception removeBook(int id)
        {
            try
            {
                string connn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(connn);
                string sqlquery = @"DELETE FROM `book` WHERE `id` = ?id;";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?id", MySqlDbType.Int16).Value = id;
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
        public Exception updateToDb()
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = @"UPDATE `book` SET `title`=?title,`description`=?description, `ISBN`=?ISBN,`pages`=?pages," +
                    "`image`=?image,`year`=?year,`price`=?price WHERE `id`=?id";

                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?id", MySqlDbType.Int32).Value = id;
                mySqlCommand.Parameters.Add("?title", MySqlDbType.VarChar).Value = Title;
                mySqlCommand.Parameters.Add("?ISBN", MySqlDbType.VarChar).Value = ISBN;
                mySqlCommand.Parameters.Add("?description", MySqlDbType.VarChar).Value = Description;
                mySqlCommand.Parameters.Add("?pages", MySqlDbType.Int32).Value = Pages;
                mySqlCommand.Parameters.Add("?price", MySqlDbType.Int32).Value = Price;
                mySqlCommand.Parameters.Add("?image", MySqlDbType.VarChar).Value = Image;
                mySqlCommand.Parameters.Add("?year", MySqlDbType.Datetime).Value = Year;
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
    }
}