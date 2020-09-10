using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ISP_Biblioteka.Models;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace ISP_Biblioteka.Models
{
    public class User
    {
        //Unikalus lenteleje
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }
        //Nuotraukos kelias
        public string Image { get; set; }
        // 1 - Vyras, 2 - Moteris, daugiau nera
        public int Gender { get; set; }
        // 0 - laukia email patvirtinimp
        // 1 - patvirtintas vartotojas
        // 2 - uzblokuotas vartotojas
        public int Validation { get; set; }
        // 1 - Skaitytonas
        // 2 - Bibliotekininke
        // 3 - Moderatorius
        public int Type { get; set; }

        /*public Exception insert()
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = @"INSERT INTO `user`(`username`, `password`, `name`, `surname`, `email`, `address` , `phone`, `image`, `gender`, `validation`, `type`) " +
                       "VALUES(?username,?password,?name,?surname,?email,?address,?phone,?image,?gender,?validation,?type);";

                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?rating", MySqlDbType.Float).Value = client.rating;
                mySqlCommand.Parameters.Add("?fk_user", MySqlDbType.VarChar).Value = client.fk_user;
                mySqlCommand.Parameters.Add("?rating_count", MySqlDbType.Int16).Value = client.rating_count;
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

        }*/

        public bool chechUser()
        {
            bool exist = false;
            string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = @"SELECT COUNT(`email`) as `count` FROM `user` WHERE `email` = ?email";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlCommand.Parameters.Add("?email", MySqlDbType.VarChar).Value = Email;
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt); 
            mySqlConnection.Close();

            int count = Int32.Parse( Convert.ToString(dt.Rows[0]["count"]));
            if (count != 0) exist = true;
           
            return exist;
        }
    }
}