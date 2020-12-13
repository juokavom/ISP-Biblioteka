using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace ISP_Biblioteka.Models
{
    public class Newsletter
    {

        public int ID { get; set; }
        public String Subject { get; set; }
        public String Content { get; set; }
        public DateTime Date { get; set; }

        public Newsletter() { }

        public Newsletter(DataRow row)
        {
            ID = Convert.ToInt32(row["id"]);
            Subject = Convert.ToString(row["subject"]);
            Content = Convert.ToString(row["content"]);
            Date = Convert.ToDateTime(row["date"]);
        }

        public Exception createNewsletter()
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = @"INSERT INTO `newsletter`(`id`, `subject`, `content`, `date`) " +
                       "VALUES(null,?subject,?content,NOW());";

                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?subject", MySqlDbType.String).Value = Subject;
                mySqlCommand.Parameters.Add("?content", MySqlDbType.String).Value = Content;
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