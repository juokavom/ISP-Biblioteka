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
                       "VALUES(NOW(),null,null,null,?fk_book_id,null,?fk_user_id);";

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
                string sqlquery = @"SELECT * FROM `order` WHERE `id` = ?id";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?id", MySqlDbType.VarChar).Value = ID;
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                DataTable dt = new DataTable();
                mda.Fill(dt);
                mySqlConnection.Close();

                    ID = Convert.ToInt16(dt.Rows[0]["id"]);
                    FK_book_id = Convert.ToInt32(dt.Rows[0]["fk_book_id"]);
                    FK_rating_id = Convert.ToInt32(dt.Rows[0]["fk_rating_id"]);
                    FK_user_id = Convert.ToInt32(dt.Rows[0]["fk_user_id"]);

                try
                {

                    sqlquery = @"UPDATE `order` SET `id`=?id,`borrow_date`=?borrow_date, `return_date`=?return_date,`validation_date`=?validation_date," +
                        "`fk_book_id`=?fk_book_id,`fk_rating_id`=?fk_rating_id, `fk_user_id`=?fk_user_id WHERE `id`=?id";

                    mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
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
                catch (Exception e)
            {
                Console.WriteLine(e);
                return e;
            }
        }

        public static List<Book> getBooks()
        {
            List<Book> books = new List<Book>();
            string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = @"SELECT * FROM `book`";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                books.Add(new Book(dt.Rows[i]));
            }

            return books;
        }

        public static List<Order> getOrders()
        {
            List<Order> allOrders = new List<Order>();
            string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = @"SELECT * FROM `order`";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                allOrders.Add(new Order(dt.Rows[i]));
            }

            return allOrders;
        }

        public static List<User> getUsers()
        {
            List<User> allUsers = new List<User>();
            string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = @"SELECT * FROM `user`";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                allUsers.Add(new User(dt.Rows[i]));
            }

            return allUsers;
        }
    }
}