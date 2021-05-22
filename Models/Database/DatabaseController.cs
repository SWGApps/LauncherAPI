using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace LauncherAPI.Models
{
    /// <summary>
    /// This class controls SQL executions, enabling dynamic parameters for statements
    /// </summary>
    internal class DatabaseController
    {
        MySqlConnection _conn;

        internal DatabaseController()
        {
            _conn = new DatabaseConnection().DatabaseInit();
        }

        /// <summary>
        /// Use this method for executing a SQL statement
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="data"></param>
        /// <returns>A MySQL data reader holding the query results</returns>
        internal MySqlDataReader Execute(string statement, dynamic[] data)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(statement, _conn);

                string[] words = statement.Split();
                List<string> parameters = new List<string>();

                foreach (string word in words)
                {
                    if (word.Contains("@Sanitized"))
                    {
                        if (word.Contains(","))
                        {
                            parameters.Add(word.Split("@Sanitized")[1].Split(",")[0]);
                        }
                        else if (word.Contains(")"))
                        {
                            parameters.Add(word.Split("@Sanitized")[1].Split(")")[0]);
                        }
                        else
                        {
                            parameters.Add(word.Split("@Sanitized")[1]);
                        }
                    }
                }

                // Dynamically add parameters to sanitize input
                for (int i=0; i < parameters.Count; i++)
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