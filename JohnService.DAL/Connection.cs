using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using JohnService.BO;
using JohnService.VO;

namespace JohnService.DAL
{
    public static class Connection
    {
        public static SqlConnection conn = new SqlConnection(connectionString);
        public static string connectionString = "Data Source=LAB-F08-06;Initial Catalog=JohnServicesDB;User ID=sa;Password=senai@123";
        public static void Connect()
        {
            CreateConnection();
        }
        public static void Connect(string server, string dataBase, string user, string password)
        {
            connectionString = $"Data Source={server};Initial Catalog={dataBase};User ID={user};Password={password}";
            CreateConnection();
        }
        private static void CreateConnection()
        {
            conn = new SqlConnection(connectionString);
            if (!IsConnected())
            {
                conn.Open();
            }
        }
        public static bool IsConnected() 
        {
            if (conn.State == ConnectionState.Open)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void Close()
        {
            if (IsConnected())
            {
                conn.Close();
            }
        }
        public static void AddValues()
        {
            UpdateTable(); //atualiza a tabela com novas colunas se forem necessárias para inclusão dos dados
            string query = "INSERT INTO ServicesRequest (";

            foreach (var item in ServiceReader.ServiceRequest)
            {
                query += $" {item.Key.ToString().Replace(" ","_")},";
            }
            //removendo a ultima virgula
            query = query.Remove(query.Length - 1);
            //fechando a parenteses
            query += ")";
            query += " VALUES (";
            foreach (var item in ServiceReader.ServiceRequest)
            {
                query += $" '{item.Value}',";
            }
            //removendo a ultima virgula
            query = query.Remove(query.Length - 1);
            query += ");";
            Connect();
            SqlCommand cmd = new SqlCommand(query, conn);

            
            cmd.ExecuteNonQuery();
            Close();
        }
        public static void UpdateValues() { }
        public static void DeleteValues() { }
        private static bool ExistTable()
        {
            string query = $"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'dbo' AND table_name = 'ServicesRequest'";
            SqlCommand cmd = new SqlCommand(query, conn);
            
            int count = (int)cmd.ExecuteScalar();

            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static void CreateTable() 
        {
            Connect ();
            if (!ExistTable())
            {
                string query = "CREATE TABLE ServicesRequest( id INT IDENTITY(1,1) PRIMARY KEY);";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();
            }
            Close();
        }
        private static void UpdateTable() 
        {
            CreateTable();

            string query = "ALTER TABLE ServicesRequest ADD";
            Connect();//abre a conexão para o uso do método ExistColumn
            foreach (var item in ServiceReader.ServiceRequest)
            {
                if (!ExistColumn(item.Key.ToString().Replace(" ", "_")))
                {

                    query += $" {item.Key.ToString().Replace(" ", "_")} VARCHAR(MAX),";
                }
                
            }
            Close ();//fecha a conexão usada por ExistColumn
            //se não foi adicionado nada, ou seja, não tem coluna nova retorna
            if (query.Equals("ALTER TABLE ServicesRequest ADD")) return;
            //trocando a ultima virgula por ponto e virgula
            query = query.Remove(query.Length - 1);
            query = query.Insert(query.Length, ";");
            Connect();//abre a conexão para o envio do comando com as novas colunas
            SqlCommand cmd = new SqlCommand(query, conn);
             
            cmd.ExecuteNonQuery();
            Close (); // fecha a conexão da atualização das colunas da tabela
        }
        private static bool ExistColumn(string columnName)
        {
            string query = $"SELECT COUNT(*) FROM information_schema.columns WHERE table_name = 'ServicesRequest' AND column_name = '{columnName}'";
            SqlCommand cmd = new SqlCommand(query, conn);
            int count = (int)cmd.ExecuteScalar();

            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
