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
    }
}