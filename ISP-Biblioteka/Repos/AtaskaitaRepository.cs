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

        public List<KnyguStatistikaViewModel1> getKnyguStatistika(DateTime? year_from, DateTime? year_to)
        {
            List<KnyguStatistikaViewModel1> knygos = new List<KnyguStatistikaViewModel1>();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = @"SELECT 
                                    bo.title,
                                    bo.year,
                                    bo.pages,
                                    bo.isbn
                                from 
                                    book bo;";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlCommand.Parameters.Add("?year_from", MySqlDbType.Int32).Value = year_from;
            mySqlCommand.Parameters.Add("?year_to", MySqlDbType.Int32).Value = year_to;
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                knygos.Add(new KnyguStatistikaViewModel1
                {
                    title = Convert.ToString(item["title"]),
                    year = Convert.ToDateTime(item["year"]),
                    pages = Convert.ToInt32(item["pages"]),
                    isbn = Convert.ToString(item["isbn"])

                }); ;
            }
            return knygos;
        }

        public List<UzsakymuIstorijaViewModel1> getUzsakymuIstorija(int? period)
        {
            List<UzsakymuIstorijaViewModel1> uzsak = new List<UzsakymuIstorijaViewModel1>();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = @"SELECT 
                                    od.borrow_date, 
                                    od.return_date,
                                    concat(us.name, ' ', us.surname)
                                from 
                                    `order` od
                                        join user us on
                                            od.fk_user_id=us.id and
                                            od.borrow_date>=IF(?period='', od.borrow_date, IFNULL(DATE_SUB(NOW(), INTERVAL ?period MONTH), od.borrow_date));";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlCommand.Parameters.Add("?period", MySqlDbType.Int32).Value = period;
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                uzsak.Add(new UzsakymuIstorijaViewModel1
                {
                    user = Convert.ToString(item["name"]),
                    borrow_date = Convert.ToDateTime(item["borrow_date"]),
                    return_date = Convert.ToDateTime(item["return_date"])

                }); ;
            }
            return uzsak;
        }

        public List<IsiskolineViewModel1> getIsiskoline(int? period)
        {
            List<IsiskolineViewModel1> isiskoline = new List<IsiskolineViewModel1>();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = @"SELECT 
                                    od.borrow_date, 
                                    od.return_date,
                                    concat(us.name, ' ', us.surname)
                                from 
                                    `order` od
                                        join user us on
                                            od.fk_user_id=us.id and
                                            od.return_date>=IF(?period='', od.return_date, IFNULL(DATE_SUB(NOW(), INTERVAL ?period MONTH), od.return_date)) and
                                            od.validation_date is null;";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlCommand.Parameters.Add("?period", MySqlDbType.Int32).Value = period;
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();

            foreach (DataRow item in dt.Rows)
            {
                isiskoline.Add(new IsiskolineViewModel1
                {
                    user = Convert.ToString(item["name"]),
                    borrow_date = Convert.ToDateTime(item["borrow_date"]),
                    return_date = Convert.ToDateTime(item["return_date"])

                }); ;
            }
            return isiskoline;
        }
    }
}