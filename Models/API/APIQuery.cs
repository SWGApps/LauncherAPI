using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using LauncherAPI.Models.Properties;

namespace LauncherAPI.Models
{
    /// <summary>
    /// This class contains methods that begin API request logic
    /// </summary>
    internal class APIQuery
    {
        DatabaseQuery _query;

        internal APIQuery()
        {
            _query = new DatabaseQuery();
        }

        /// <summary>
        /// Use this method to create SWG accounts
        /// </summary>
        /// <param name="username"></param>
        /// <param name="credentials"></param>
        /// <returns>An indented JSON formatted strong with the results of the account creation attempt</returns>
        internal string Create(string username, CreateAccountCredentials credentials)
        {
            Dictionary<string, string> types = new Dictionary<string, string>
            {
                { "UserCharacters", username },
                { "PasswordCharacters", credentials.Password },
                { "EmailCharacters", credentials.Email },
                { "DiscordCharacters", credentials.Discord },
                { "UsernameLength", username },
                { "PasswordLength", credentials.Password },
                { "EmailLength", credentials.Email },
                { "DiscordLength", credentials.Discord }
            };

            foreach (KeyValuePair<string, string> type in types)
            {
                string result = StringUtils.GetAccountCreationErrorResponse(type.Key, type.Value);

                if (result != null)
                {
                    return result;
                }
            }

            if (credentials.Password != credentials.PasswordConfirmation)
            {
                return JsonConvert.SerializeObject(new CreateAccountResponse {
                    Result = "PasswordMisMatch"
                }, Formatting.Indented);
            }

            bool accountExists = _query.CheckIfAccountExists(username.Trim().ToLower());

            if (accountExists)
            {
                return JsonConvert.SerializeObject(new CreateAccountResponse {
                    Result = "AccountAlreadyExists"
                }, Formatting.Indented);
            }

            // Check if captcha is correct server-side
            if (int.Parse(credentials.CaptchaValue1) + 
                int.Parse(credentials.CaptchaValue2) != 
                int.Parse(credentials.CaptchaAnswer))
            {
                return JsonConvert.SerializeObject(new CreateAccountResponse {
                    Result = "CaptchaAnswerIncorrect"
                }, Formatting.Indented);                
            }

            if (string.IsNullOrEmpty(credentials.Discord))
            {
                credentials.Discord = "";
            }
            
            bool accountInserted = _query.InsertAccount(new List<string>() {
                username.Trim().ToLower(), 
                credentials.Password.Trim(),
                credentials.Email.Trim(),
                credentials.Discord.Trim(),
                credentials.SubscribeToNewsletter.ToString()
            });

            if (accountInserted)
            {
                return JsonConvert.SerializeObject(new CreateAccountResponse {
                    Result = "Success"
                }, Formatting.Indented);
            }
            else
            {
                return JsonConvert.SerializeObject(new CreateAccountResponse {
                    Result = "Database insertion error, please report this to staff."
                }, Formatting.Indented);
            }
        }

        /// <summary>
        /// Use this method when a login request comes in without a username
        /// </summary>
        /// <returns>An invalid credentials error</returns>
        internal string CreateEmpty()
        {
            return JsonConvert.SerializeObject(new LoginResponse {
                Result = "InvalidInputProvided",
                Username = "",
                Characters = new List<string>()
            }, Formatting.Indented);
        }

        /// <summary>
        /// Use this method to login using a previously created SWG account
        /// </summary>
        /// <param name="username"></param>
        /// <param name="credentials"></param>
        /// <returns>An indented JSON formatted string with the results of the login attempt</returns>
        internal string Login(string username, LoginCredentials credentials)
        {
            // Tuple, from database
            (string password, string salt) = _query.GetUserCredentials(username.Trim().ToLower());

            if (password == "DatabaseConnectionError")
            {
                return JsonConvert.SerializeObject(new LoginResponse {
                    Result = "DatabaseConnectionError",
                    Username = "",
                    Characters = new List<string>()
                }, Formatting.Indented);
            }

            if (!String.IsNullOrEmpty(password) && !String.IsNullOrEmpty(salt))
            {
                string passwordInput;
                passwordInput = credentials.Password;
                
                (string hashedPasswordInput, string returnedSalt) = SWGUtils.HashPassword(passwordInput, salt);

                if (password == hashedPasswordInput.Trim())
                {
                    List<string> characters = _query.GetCharacters(username.Trim().ToLower());

                    return JsonConvert.SerializeObject(new LoginResponse {
                        Result = "Success",
                        Username = username.Trim().ToLower(),
                        Characters = characters
                    }, Formatting.Indented);
                }
            }

            return JsonConvert.SerializeObject(new LoginResponse {
                Result = "InvalidCredentials",
                Username = username.Trim().ToLower(),
                Characters = new List<string>()
            }, Formatting.Indented);
        }

        /// <summary>
        /// Use this method when a login request comes in without a username
        /// </summary>
        /// <returns>An invalid credentials error</returns>
        internal string LoginEmpty()
        {
            return JsonConvert.SerializeObject(new LoginResponse {
                Result = "InvalidCredentials",
                Username = "",
                Characters = new List<string>()
            }, Formatting.Indented);
        }
    }
}
