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
    public class NeaktyvusVartotojaiRepository
    {
        public List<NeaktyvusVartotojaiViewModel> getNeaktyvusVartotojai(/*DateTime? nuo, DateTime? iki, String darbuotojas, String valiuta, String numeris*/)
        {
            List<NeaktyvusVartotojaiViewModel> sutartys = new List<NeaktyvusVartotojaiViewModel>();
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            string sqlquery = @"SELECT name, surname from user;";
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
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
                    surname = Convert.ToString(item["surname"])

                }); ;
            }
            return sutartys;
        }
    }
}