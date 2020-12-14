using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace ISP_Biblioteka.Models
{
    public class Author
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

    public Author()
    {
    }

    public Author(DataRow row)
    {
        ID = Convert.ToInt16(row["id"]);
        Name = Convert.ToString(row["name"]);
        Surname = Convert.ToString(row["surname"]);
    }

        public Exception insertToDb()
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = @"INSERT INTO `author`(`id`, `name`, `surname`) " +
                       "VALUES(null,?name,?surname);";

                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?name", MySqlDbType.VarChar).Value = Name;
                mySqlCommand.Parameters.Add("?surname", MySqlDbType.VarChar).Value = Surname;
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