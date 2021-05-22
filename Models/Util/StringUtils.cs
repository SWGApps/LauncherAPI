using System.Text.RegularExpressions;

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
    }
}
