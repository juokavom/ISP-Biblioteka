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

        public int ID { get; set; }
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

        public User()
        {
            
        }

        public User(DataRow row)
        {
            ID = Convert.ToInt16(row["id"]);
            Email = Convert.ToString(row["email"]);
            Password = Convert.ToString(row["password"]);
            Name = Convert.ToString(row["name"]);
            Surname = Convert.ToString(row["surname"]);
            Phone = Convert.ToString(row["phone"]);
            Address = Convert.ToString(row["address"]);
            Image = Convert.ToString(row["image"]);
            Gender = Convert.ToInt16(row["gender"] == DBNull.Value ? 0 : row["gender"]);
            Validation = Convert.ToInt16(row["validation"] == DBNull.Value ? 0 : row["validation"]);
            Type = Convert.ToInt16(row["type"] == DBNull.Value ? 0 : row["type"]);
        }
        public Exception insertToDb()
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = @"INSERT INTO `user`(`id`, `password`, `name`, `surname`, `email`, `address` , `phone`, `image`, `gender`, `validation`, `type`) " +
                       "VALUES(null,?password,?name,?surname,?email,?address,?phone,?image,?gender,?validation,?type);";

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
        public Exception updateToDb()
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = @"UPDATE `user` SET `name`=?name,`surname`=?surname, `address`=?address,`phone`=?phone,"+
                    "`image`=?image,`gender`=?gender,`validation`=?validation,`type`=?type WHERE `id`=?id";

                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?id", MySqlDbType.VarChar).Value = ID;
                mySqlCommand.Parameters.Add("?name", MySqlDbType.VarChar).Value = Name;
                mySqlCommand.Parameters.Add("?surname", MySqlDbType.VarChar).Value = Surname;
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
                    ID = Convert.ToInt16(dt.Rows[0]["id"]);
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

        public static Exception removeUser(int id)
        {
            try
            {
                string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = @"DELETE FROM `user` WHERE `id` = ?id;";
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

        public Exception logSession(string ip, string browser)
        {
            try
            {
                this.updateValues();
                string conn = ConfigurationManager.ConnectionStrings["Mysqlconnection"].ConnectionString;
                MySqlConnection mySqlConnection = new MySqlConnection(conn);
                string sqlquery = @"SET @@session.time_zone='+02:00';INSERT INTO `log`(`id`, `ip`, `browser_type`, `date`, `fk_user_id`) " +
                    "VALUES (null,?ip,?browser,NOW(),?user)";
                MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
                mySqlCommand.Parameters.Add("?ip", MySqlDbType.VarChar).Value = ip;
                mySqlCommand.Parameters.Add("?browser", MySqlDbType.VarChar).Value = browser;
                mySqlCommand.Parameters.Add("?user", MySqlDbType.VarChar).Value = this.ID;
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