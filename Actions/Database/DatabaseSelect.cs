using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace LauncherAPI.Controllers
{
    internal class DatabaseSelect
    {
        MySqlConnection _conn;

        internal DatabaseSelect()
        {
            _conn = new DatabaseConnection().DatabaseInit();
        }

        internal MySqlDataReader Execute(string statement, dynamic[] data)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(statement, _conn);

                string[] parameters = statement.Split("@")[1].Split("Sanitized")[1].Split(")")[0].Split(" ");

                // Dynamically add parameters to sanitize input
                for (int i=0; i < parameters.Length; i++)
                {
                    cmd.Parameters.AddWithValue($"@Sanitized{parameters[i]}", data[i]);
                }
                
                MySqlDataReader reader = cmd.ExecuteReader();

                return reader;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }
    }
}