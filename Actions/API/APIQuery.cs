using System;

namespace LauncherAPI.Controllers
{
    internal class APIQuery
    {
        DatabaseSelect _select;
        private string _password;
        private string _salt;

        internal APIQuery()
        {
            _select = new DatabaseSelect();
        }

        internal Tuple<string, string> GetUserCredentials(string username)
        {
            try
            {
                string[] data = { username };
                
                var result = _select.Execute("SELECT password, salt FROM accounts WHERE username=@SanitizedUsername", data);

                while (result.Read())
                {
                    _password = (string)result[0];
                    _salt = (string)result[1];
                }

                result.Close();

                return Tuple.Create(_password, _salt);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return Tuple.Create("", "");
        }

    }
}