using System;
using MySql.Data.MySqlClient;

namespace LauncherAPI.Controllers
{
    internal class DatabaseConnection
    {
        string server = "localhost";
        string user = "swgemu";
        string database = "swgemu";
        int port = 3306;
        string password = "123456";
        
        private MySqlConnection _conn;

        internal DatabaseConnection()
        {
            string connStr = $"server={server};user={user};database={database};port={port};password={password}";
            _conn = new MySqlConnection(connStr);
        }

        internal MySqlConnection DatabaseInit()
        {
            try
            {
                _conn.Open();
                return _conn;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}

