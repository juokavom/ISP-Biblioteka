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
    public class User
    {
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        //Unikalus lenteleje
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        //Nuotraukos kelias
        public string Image { get; set; }
        // 1 - Vyras, 2 - Moteris, daugiau nera
        public int Gender { get; set; }
        // 0 - laukia email patvirtinimp
        // 1 - patvirtintas vartotojas
        // 2 - uzblokuotas vartotojas
        public int Validation { get; set; }
        // 1 - Skaitytojas
        // 2 - Bibliotekininke
        // 3 - Moderatorius
        public int Type { get; set; }

        public Exception insertToDb()
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = @"INSERT INTO `user`(`password`, `name`, `surname`, `email`, `address` , `phone`, `image`, `gender`, `validation`, `type`) " +
                       "VALUES(?password,?name,?surname,?email,?address,?phone,?image,?gender,?validation,?type);";

                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?password", MySqlDbType.VarChar).Value = Password;
                mySqlCommand.Parameters.Add("?name", MySqlDbType.VarChar).Value = Name;
                mySqlCommand.Parameters.Add("?surname", MySqlDbType.VarChar).Value = Surname;
                mySqlCommand.Parameters.Add("?email", MySqlDbType.VarChar).Value = Email;
                mySqlCommand.Parameters.Add("?address", MySqlDbType.VarChar).Value = Address;
                mySqlCommand.Parameters.Add("?phone", MySqlDbType.VarChar).Value = Phone;
                mySqlCommand.Parameters.Add("?image", MySqlDbType.VarChar).Value = Image;
                mySqlCommand.Parameters.Add("?gender", MySqlDbType.Int32).Value = Gender;
                mySqlCommand.Parameters.Add("?validation", MySqlDbType.Int32).Value = Validation;
                mySqlCommand.Parameters.Add("?type", MySqlDbType.Int32).Value = Type;
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
                string sqlquery = @"SELECT * FROM `user` WHERE `email` = ?email";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?email", MySqlDbType.VarChar).Value = Email;
                mySqlConnection.Open();
                mySqlCommand.ExecuteNonQuery();
                MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
                DataTable dt = new DataTable();
                mda.Fill(dt);
                mySqlConnection.Close();

                if (dt.Rows.Count == 1)
                {
                    Password = Convert.ToString(dt.Rows[0]["password"]);
                    Name = Convert.ToString(dt.Rows[0]["name"]);
                    Surname = Convert.ToString(dt.Rows[0]["surname"]);
                    Phone = Convert.ToString(dt.Rows[0]["phone"]);
                    Address = Convert.ToString(dt.Rows[0]["address"]);
                    Image = Convert.ToString(dt.Rows[0]["image"]);
                    Gender = Convert.ToInt16(dt.Rows[0]["gender"] == DBNull.Value ? 0 : dt.Rows[0]["gender"]);
                    Validation = Convert.ToInt16(dt.Rows[0]["validation"] == DBNull.Value ? 0 : dt.Rows[0]["validation"]);
                    Type = Convert.ToInt16(dt.Rows[0]["type"] == DBNull.Value ? 0 : dt.Rows[0]["type"]);
                    return null;
                }
                else if (dt.Rows.Count > 1)
                {
                    throw new Exception("DB keli useriai su tokiu emailu");
                }
                throw new Exception("DB nera userio su tokiu emailu");
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return e;
            }
        }

        public static User loginCheck(User user)
        {
            User ret = null;

            string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = @"SELECT COUNT(`email`) as `count` FROM `user` WHERE `email` = ?email AND `password` = ?password AND `validation` = 1";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlCommand.Parameters.Add("?email", MySqlDbType.VarChar).Value = user.Email;
            mySqlCommand.Parameters.Add("?password", MySqlDbType.VarChar).Value = user.Password;
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            int count = Int32.Parse(Convert.ToString(dt.Rows[0]["count"]));
            if (count == 1)
            {
                user.updateValues();
                ret = user;
            }

            return ret;
        }

        public static Exception validate(string email, int number)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = @"UPDATE `user` SET `validation`=?validation WHERE `email`=?email;";

                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?email", MySqlDbType.VarChar).Value = email;
                mySqlCommand.Parameters.Add("?validation", MySqlDbType.Int32).Value = number;
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

        public static Exception changePassword(string email, string password)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = @"UPDATE `user` SET `password`=?password WHERE `email`=?email;";

                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?email", MySqlDbType.VarChar).Value = email;
                mySqlCommand.Parameters.Add("?password", MySqlDbType.VarChar).Value = password;
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

        public static bool chechUniqueEmail(string email)
        {
            bool exist = false;
            string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = @"SELECT COUNT(`email`) as `count` FROM `user` WHERE `email` = ?email";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlCommand.Parameters.Add("?email", MySqlDbType.VarChar).Value = email;
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            int count = Int32.Parse(Convert.ToString(dt.Rows[0]["count"]));
            if (count != 0) exist = true;

            return exist;
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
        public string getUserTypeName()
        {
            string type;
            if (Type == 1) type = "Skaitytojas";
            else
            {
                type = Type == 2 ? "Bibliotekininkas" : "Moderatorius";
            }
            return type;
        }
    }
}