using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using JohnService.BO;
using JohnService.VO;

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

            string query = "INSERT INTO ServicesRequest (";

            foreach (var item in ServiceReader.ServiceRequest)
            {
                query += $" {item.Key},";
            }
            //removendo a ultima virgula
            query = query.Remove(query.Length - 1);
            //fechando a parenteses
            query += ")";
            query += " VALUES (";
            foreach (var item in ServiceReader.ServiceRequest)
            {
                query += $" {item.Value},";
            }


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
            string query = "CREATE TABLE ServicesRequest";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();
        }
        public void UpdateTable() 
        {
            string query = "ALTER TABLE ServicesRequest ";
            foreach (var item in ServiceReader.ServiceRequest)
            {
                query += $" ADD {item.Key} VARCHAR(MAX),";
            }
            //trocando a ultima virgula por ponto e virgula
            query = query.Remove(query.Length - 1);
            query = query.Insert(query.Length, ";");

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.ExecuteNonQuery();

        }
    }
}
