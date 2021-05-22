using System;
using MySql.Data.MySqlClient;
using LauncherAPI.Models.Properties;

namespace LauncherAPI.Models
{
    /// <summary>
    /// This class contains information and methods for connecting to the database
    /// </summary>
    public class DatabaseConnection
    {
        string server = DatabaseProperties.DBHost;
        string user = DatabaseProperties.DBUser;
        string database = DatabaseProperties.DBDatabase;
        int port = DatabaseProperties.DBPort;
        string password = DatabaseProperties.DBPassword;
        
        private MySqlConnection _conn;

        public DatabaseConnection()
        {
            string connStr = $"server={server};user={user};database={database};port={port};password={password}";
            _conn = new MySqlConnection(connStr);
        }

        /// <summary>
        /// Use this method to initiate a connection to the database
        /// </summary>
        /// <returns>A newly opened connection to the database</returns>
        public MySqlConnection DatabaseInit()
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

