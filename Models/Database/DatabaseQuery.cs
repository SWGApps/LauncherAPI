using System;
using System.Collections.Generic;

namespace LauncherAPI.Models
{
    /// <summary>
    /// This class is for initiating querys for information from the database
    /// </summary>
    internal class DatabaseQuery
    {
        /// <summary>
        /// Use this method to insert a SWG account into the database
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns>True if account creation was successful</returns>
        internal bool InsertAccount(List<string> credentials)
        {
            DatabaseController insert = new DatabaseController();

            string username = credentials[0];
            string password = credentials[1];
            string email = credentials[2];
            string discord = credentials[3];
            string subscribed = credentials[4];

            int isSubscribed = (subscribed.ToLower().Trim() == "true") ? 1 : 0;

            try
            {
                (string hashedPassword, string salt) = SWGUtils.HashPassword(password);

                string[] data = { username, hashedPassword, email, discord, subscribed };

                var result = insert.Execute(
                    $"INSERT into accounts (username, password, station_id, salt, email, discord, subscribed) VALUES (@SanitizedUsername, @SanitizedPassword," +
                    $"'{ SWGUtils.GenerateStationId() }', '{ salt }', @SanitizedEmail, @SanitizedDiscord, { subscribed })", data
                );
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return false;
        }

        /// <summary>
        /// Use this method to check if an account exists before creating a new one
        /// </summary>
        /// <param name="username"></param>
        /// <returns>True if the username passed exists in the database</returns>
        internal bool CheckIfAccountExists(string username)
        {
            DatabaseController select = new DatabaseController();

            try
            {
                string[] data = { username };
                
                var result = select.Execute("SELECT username FROM accounts WHERE username=@SanitizedUsername", data);

                if (result.HasRows) 
                {
                    result.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return false;
        }

        /// <summary>
        /// Use this method to get the users current credentials for checking on login
        /// </summary>
        /// <param name="username"></param>
        /// <returns>A tuple with the password and salt strings</returns>
        internal Tuple<string, string> GetUserCredentials(string username)
        {
            string password = "";
            string salt = "";
            DatabaseController select = new DatabaseController();

            try
            {
                string[] data = { username };
                
                var result = select.Execute("SELECT password, salt FROM accounts WHERE username=@SanitizedUsername", data);

                while (result.Read())
                {
                    password = (string)result[0];
                    salt = (string)result[1];
                }

                result.Close();

                return Tuple.Create(password, salt);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return Tuple.Create("DatabaseConnectionError", "");
        }

        /// <summary>
        /// Use this method to get a list of characters on an account on login
        /// </summary>
        /// <param name="username"></param>
        /// <returns>A list of strings containing the users character names</returns>
        internal List<string> GetCharacters(string username)
        {
            List<string> characters = new List<string>();
            DatabaseController select = new DatabaseController();

            try
            {
                string[] data = { username };
                
                var result = select.Execute("SELECT firstname FROM characters WHERE account_id IN (SELECT account_id FROM accounts WHERE username=@SanitizedUsername)", data);

                while (result.Read())
                {
                    characters.Add((string)result[0]);
                }

                result.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
            return characters;
        }
    }
}