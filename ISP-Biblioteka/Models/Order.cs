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
        public int FK_book_id { get; set; }
        public int FK_rating_id { get; set; }
        public int FK_user_id { get; set; }

        public Order() { }

        public Order(DataRow row)
        {
            ID = Convert.ToInt32(row["id"]);
            Borrow_date = Convert.ToDateTime(row["borrow_date"]);
            Return_date = Convert.ToDateTime(row["return_date"]);
            Validation_date = Convert.ToDateTime(row["validation_date"]);
            FK_book_id = Convert.ToInt32(row["fk_book_id"]);
            FK_rating_id = Convert.ToInt32(row["fk_rating_id"]);
            FK_user_id = Convert.ToInt32(row["fk_user_id"]);
        }

        public Exception createOrderRequest()
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = @"INSERT INTO `order`(`id`, `borrow_date`, `return_date`, `validation_date`, `fk_book_id`, `fk_rating_id`, `fk_user_id`) " +
                       "VALUES(null,null,null,null,?fk_book_id,null,?fk_user_id);";

                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?fk_book_id", MySqlDbType.Int32).Value = FK_book_id;
                mySqlCommand.Parameters.Add("?fk_user_id", MySqlDbType.Int32).Value = FK_book_id;
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

        public Exception validateOrder()
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = @"UPDATE `order` SET `id`=?id,`borrow_date`=?borrow_date, `return_date`=?return_date,`validation_date`=?validation_date," +
                    "`fk_book_id`=?fk_book_id,`fk_rating_id`=?fk_rating_id, `fk_user_id`=?fk_user_id WHERE `id`=?id";

                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?id", MySqlDbType.Int32).Value = ID;
                mySqlCommand.Parameters.Add("?borrow_date", MySqlDbType.Date).Value = Borrow_date;
                mySqlCommand.Parameters.Add("?return_date", MySqlDbType.Date).Value = Return_date;
                mySqlCommand.Parameters.Add("?validation_date", MySqlDbType.Date).Value = Validation_date;
                mySqlCommand.Parameters.Add("?fk_book_id", MySqlDbType.Int32).Value = FK_book_id;
                mySqlCommand.Parameters.Add("?fk_rating_id", MySqlDbType.Int32).Value = FK_rating_id;
                mySqlCommand.Parameters.Add("?fk_user_id", MySqlDbType.Int32).Value = FK_user_id;
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