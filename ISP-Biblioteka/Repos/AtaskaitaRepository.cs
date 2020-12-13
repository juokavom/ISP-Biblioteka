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
                                    DATE_FORMAT(lo.date,'%Y-%m-%d') date
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
                body += "<th>" + "Paskutinio prisijungimo data" + "</th>";
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

        public string getAtaskaitos2()
        {
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = @"SELECT
                                    DATE_FORMAT(od.validation_date,'%Y-%m-%d') validation_date,
                                    DATE_FORMAT(od.borrow_date,'%Y-%m-%d') borrow_date, 
                                    DATE_FORMAT(od.return_date,'%Y-%m-%d') return_date,
                                    concat(us.name, ' ', us.surname) name,
                                    bo.title
                                from 
                                    `order` od
                                        join user us on
                                            od.fk_user_id=us.id
                                        join book bo on
                                            od.fk_book_id =  bo.id";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();
            string body = " <h1>Knygų pasiėmimai per pastarąjį 1 mėnesį</h1>";
            if (dt.Rows.Count > 0)
            {
                body += "<table border=" + 1 + ">";
                body += "<tr>";
                body += "<th>" + "Vartotojas" + "</th>";
                body += "<th>" + "Knyga" + "</th>";
                body += "<th>" + "Pasiėmimo data" + "</th>";
                body += "<th>" + "Grąžinti iki" + "</th>";
                body += "<th>" + "Grąžinimo data" + "</th>";
                body += "</tr>";


                foreach (DataRow item in dt.Rows)
                {
                    body += "<tr>";
                    body += "<td>" + item["name"] + "</td>";
                    body += "<td>" + item["title"] + "</td>";
                    body += "<td>" + item["borrow_date"] + "</td>";
                    body += "<td>" + item["return_date"] + "</td>";
                    body += "<td>" + item["validation_date"] + "</td>";
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

        public Dictionary<int,string> getUsers()
        {
            Dictionary<int,string> users = new Dictionary<int, string>();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = @"SELECT distinct
                                    concat(us.name, ' ', us.surname) user,
                                    us.id
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
                users.Add(Convert.ToInt32(item["id"]), Convert.ToString(item["user"]));
            }
            return users;
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
                                    book bo
                                where
                                    bo.year>=IF(?year_from='', bo.year, IFNULL(?year_from, bo.year)) and
                                    bo.year<=IF(?year_to='', bo.year, IFNULL(?year_to, bo.year));";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlCommand.Parameters.Add("?year_from", MySqlDbType.Datetime).Value = year_from;
            mySqlCommand.Parameters.Add("?year_to", MySqlDbType.DateTime).Value = year_to;
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

        public List<UzsakymuIstorijaViewModel1> getUzsakymuIstorija(int? period, int? user)
        {
            List<UzsakymuIstorijaViewModel1> uzsak = new List<UzsakymuIstorijaViewModel1>();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = @"SELECT
                                    od.validation_date,
                                    od.borrow_date, 
                                    od.return_date,
                                    concat(us.name, ' ', us.surname) name,
                                    bo.title
                                from 
                                    `order` od
                                        join user us on
                                            od.fk_user_id=us.id
                                        join book bo on
                                            od.fk_book_id =  bo.id
                                where
                                    od.borrow_date>=IF(?period='', od.borrow_date, IFNULL(DATE_SUB(NOW(), INTERVAL ?period MONTH), od.borrow_date)) and
                                    us.id=IF(?user='', us.id, IFNULL( ?user, us.id));";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlCommand.Parameters.Add("?period", MySqlDbType.Int32).Value = period;
            mySqlCommand.Parameters.Add("?user", MySqlDbType.Int32).Value = user;
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
                    return_date = Convert.ToDateTime(item["return_date"]),
                    book = Convert.ToString(item["title"]),
                    validation_date = Convert.ToDateTime(item["validation_date"])
                }); ;
            }
            return uzsak;
        }

        public List<MetMenAtaskaitaViewModel1> getMetMenAtaskaita(int? period)
        {
            List<MetMenAtaskaitaViewModel1> uzsak = new List<MetMenAtaskaitaViewModel1>();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = @"SELECT 
                                    od.borrow_date,
                                    od.return_date, 
                                    concat(us.name, ' ', us.surname) name,
                                    bo.title
                                from 
                                    `order` od
                                        left join user us on
                                            od.fk_user_id=us.id
                                        left join book bo on
                                            od.fk_book_id = bo.id
                                where
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
                uzsak.Add(new MetMenAtaskaitaViewModel1
                {
                    user = Convert.ToString(item["name"]),
                    borrow_date = Convert.ToDateTime(item["borrow_date"]),
                    return_date = Convert.ToDateTime(item["return_date"]),
                    book = Convert.ToString(item["title"])

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
                                    concat(us.name, ' ', us.surname) name,
                                    bo.title
                                from 
                                    `order` od
                                        join user us on
                                            od.fk_user_id=us.id
                                        left join book bo on
                                            od.fk_book_id = bo.id
                                where
                                    od.return_date>=IF(?period='', od.return_date, IFNULL(DATE_SUB(NOW(), INTERVAL ?period MONTH), od.return_date)) and
                                    od.validation_date = '0000-00-00';
                                            ";
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
                    book = Convert.ToString(item["title"]),
                    borrow_date = Convert.ToDateTime(item["borrow_date"]),
                    return_date = Convert.ToDateTime(item["return_date"])

                }); ;
            }
            return isiskoline;
        }
    }
}