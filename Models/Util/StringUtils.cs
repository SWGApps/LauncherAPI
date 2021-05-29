using System;
using System.Globalization;
using System.Text.RegularExpressions;
using LauncherAPI.Models.Properties;
using Newtonsoft.Json;

namespace LauncherAPI.Models
{
    /// <summary>
    /// This class contains methods for checking strings for valid characters
    /// </summary>
    internal static class StringUtils
    {
        /// <summary>
        /// Use this method to check if a string is alphanumeric
        /// </summary>
        /// <param name="text"></param>
        /// <returns>True if the string passed is alphanumeric</returns>
        static internal bool IsAlphaNum(string text)
        {
            return Regex.IsMatch(text, "^[a-zA-Z0-9]+$"); 
        }

        /// <summary>
        /// Use this method to check if a string is a valid password, not containing bad special characters
        /// </summary>
        /// <param name="text"></param>
        /// <returns>True if the string passed is a valid password</returns>
        static internal bool IsValidPassword(string text)
        {
            return Regex.IsMatch(text, "^[a-zA-Z0-9 !@#$&]+$");
        }

        static internal bool IsValidDiscord(string text)
        {
            if (text == "everyone")
                return false;
            if (text == "here")
                return false;
            if (text =="discordtag")
                return false;

            if (!text.Contains("#"))
                return false;
            
            string tag = text.Split("#")[1];

            if (tag.Length != 4)
                return false;

            bool isTagNumber = int.TryParse(tag, out int number);

            if (!isTagNumber)
                return false;

            return Regex.IsMatch(text, "^.{3,32}#[0-9]{4}$");
        }

        static internal bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        static internal string GetAccountCreationErrorResponse(string type, string text)
        {
            int userChars = 6;
            int passChars = 8;
            int emailChars = 9;
            int discordChars = 6;

            if (text != null)
            {
                if (type == "UserLength")
                    if (text.Length < userChars) return GetErrorResponse("Username", $"Requires{userChars}Characters");
                if (type == "PassLength")
                    if (text.Length < passChars) return GetErrorResponse("Password", $"Requires{passChars}Characters");
                if (type == "EmailLength")
                    if (text.Length < emailChars) return GetErrorResponse("Email", $"Requires{emailChars}Characters");
                if (type == "DiscordLength")
                    if (text.Length < discordChars) return GetErrorResponse("Discord", $"Requires{discordChars}Characters");
                if (type == "UserCharacters")
                    if (!IsAlphaNum(text)) return GetErrorResponse("Username", "ContainsInvalidCharacters");
                if (type == "PasswordCharacters")
                    if (!IsValidPassword(text)) return GetErrorResponse("Password", "ContainsInvalidCharacters");
                if (type == "EmailCharacters")
                    if (!IsValidEmail(text)) return GetErrorResponse("Email", "ContainsInvalidCharacters");
                if (type == "DiscordCharacters")
                    if (!IsValidDiscord(text)) return GetErrorResponse("Discord", "ContainsInvalidCharacters");
            }

            return null;
        }
        
        static string GetErrorResponse(string type, string reason)
        {
            return JsonConvert.SerializeObject(new CreateAccountResponse {
                Result = $"{type}{reason}"
            }, Formatting.Indented);
        }
    }
}
