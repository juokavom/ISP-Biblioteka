using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using ISP_Biblioteka.ViewModels;
using ISP_Biblioteka.Models;
using MySql.Data.MySqlClient;


namespace ISP_Biblioteka.Repos
{
    public class AtaskaitaRepository
    {
        public List<NeaktyvusVartotojaiViewModel> getNeaktyvusVartotojai(int? period /*DateTime? iki, String darbuotojas, String valiuta, String numeris*/)
        {
            List<NeaktyvusVartotojaiViewModel> sutartys = new List<NeaktyvusVartotojaiViewModel>();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = @"SELECT 
                                    us.name, 
                                    us.surname,
                                    us.email,
                                    lo.date
                                from 
                                    user us
                                        join log lo on
                                            us.id = lo.fk_user_id
                                where
                                    lo.date=(select max(date) from log where fk_user_id = us.id) and
                                    lo.date<=IF(?period='', lo.date, IFNULL(DATE_SUB(NOW(), INTERVAL ?period MONTH), lo.date));";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlCommand.Parameters.Add("?period", MySqlDbType.Int32).Value = period;
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                sutartys.Add(new NeaktyvusVartotojaiViewModel
                {
                    name = Convert.ToString(item["name"]),
                    surname = Convert.ToString(item["surname"]),
                    email = Convert.ToString(item["email"]),
                    last_login = Convert.ToDateTime(item["date"])

                }); ;
            }
            return sutartys;
        }

        public string getAtaskaitos()
        {
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = @"SELECT 
                                    us.name, 
                                    us.surname,
                                    us.email,
                                    lo.date
                                from 
                                    user us
                                        join log lo on
                                            us.id = lo.fk_user_id
                                where
                                    lo.date=(select max(date) from log where fk_user_id = us.id) and
                                    lo.date<=DATE_SUB(NOW(), INTERVAL 6 MONTH);";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();
            string body = " <h1>Neprisijungę vartotojai per pastaruosius 6 mėnesius</h1>";
            if (dt.Rows.Count>0)
            {
                body += "<table border=" + 1 + ">";
                body += "<tr>";
                body += "<th>" + "Vardas" + "</th>";
                body += "<th>" + "Pavardė" + "</th>";
                body += "<th>" + "El. paštas" + "</th>";
                body += "<th>" + "Prisijungimo data" + "</th>";
                body += "</tr>";


                foreach (DataRow item in dt.Rows)
                {
                    body += "<tr>";
                    body += "<td>" + item["name"] + "</td>";
                    body += "<td>" + item["surname"] + "</td>";
                    body += "<td>" + item["email"] + "</td>";
                    body += "<td>" + item["date"] + "</td>";
                    body += "</tr>";
                }
                body += "</table>";
            }
            else
            {
                body += "<table border=" + 1 + ">";
                body += "<tr>";
                body += "<th>" + "Nerasta jokių rezultatų!" + "</th>";
                body += "</tr>";
                body += "</table>";
            }
            

            return body;
        }

        public List<string> getEmail()
        {
            List<string> email = new List<string>();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = @"SELECT distinct
                                    us.email
                                from 
                                    user us;";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                email.Add(Convert.ToString(item["email"]));
            }
            return email;
        }
    }
}