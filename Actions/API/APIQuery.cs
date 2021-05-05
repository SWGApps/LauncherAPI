using System;
using System.Collections.Generic;

namespace LauncherAPI.Controllers
{
    internal class APIQuery
    {
        internal Tuple<string, string> GetUserCredentials(string username)
        {
            string password = "";
            string salt = "";
            DatabaseSelect select = new DatabaseSelect();

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

            return Tuple.Create("", "");
        }

        internal List<string> GetCharacters(string username)
        {
            
            List<string> characters = new List<string>();
            DatabaseSelect select = new DatabaseSelect();

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