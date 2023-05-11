using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace JohnService.DAL
{
    public class Connection
    {
        public static SqlConnection conn { get; set; }
        public static string connectionString = null;
        public void Connect()
        {
            string connectionString = "Data Source=LAB-F08-06;Initial Catalog=JohnServicesDB;User ID=sa;Password=senai@123";
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
        }
        public void AddValues()
        {

            string query = "INSERT INTO nome_tabela (coluna1, coluna2, coluna3) VALUES (@valor1, @valor2, @valor3)";

            SqlCommand cmd = new SqlCommand(query, conn);
            //cmd.Parameters.AddWithValue("@valor1", valor1);
           // cmd.Parameters.AddWithValue("@valor2", valor2);
           // cmd.Parameters.AddWithValue("@valor3", valor3);

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public void UpdateValues() { }
        public void DeleteValues() { }
        public void CreateTable() 
        {
            string query = "CREATE TABLE ServicesRequest (coluna1, coluna2, coluna3) VALUES (@valor1, @valor2, @valor3)";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
        }
    }
}
