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
    public class Books
    {
        public static List<Book> getBooks()
        {
            List<Book> allBooks = new List<Book>();
            string connn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(connn);
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
                allBooks.Add(new Book(dt.Rows[i]));
            }

            return allBooks;
        }
    }
}