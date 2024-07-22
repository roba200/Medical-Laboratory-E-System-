using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Laboratory_Management_System.Data
{
    internal class Connection
    {
        private static readonly string server = "localhost";
        private static readonly string database = "lab";
        private static readonly string uid = "root";
        private static readonly string password = "123456";
        private static readonly string connectionString = $"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};";

        public static MySqlConnection dataSource()
        {
            return new MySqlConnection(connectionString);
        }

        public void connOpen(MySqlConnection conn)
        {
            if (conn != null && conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
        }

        public void connClose(MySqlConnection conn)
        {
            if (conn != null && conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }
    }
}
