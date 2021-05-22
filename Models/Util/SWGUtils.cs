using System;
using System.Text;
using System.Security.Cryptography;
using LauncherAPI.Models.Properties;

namespace LauncherAPI.Models
{
    internal static class SWGUtils
    {
        /// <summary>
        /// Use this method to generate a ten digit Station ID
        /// </summary>
        /// <returns>A ten digit station ID</returns>
        internal static int GenerateStationId()
        {
            var rand = new Random();
            return rand.Next(1000000000, 2147483647);
        }

        /// <summary>
        /// Use this method to generate a salt string
        /// </summary>
        /// <returns>A 32 character salt string</returns>
        internal static string GenerateSalt()
        {
            Byte[] bytes = new byte[32];

            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(bytes);
            }

            StringBuilder sb = new StringBuilder();

            // Convert bytes to UTF-16 string
            foreach (Byte b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }

            // Shorten to 32 characters
            return sb.ToString().Substring(0, 32);
        }

        /// <summary>
        /// Use this method to obtain a hashed password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns>A hashed password string using the salt provided, or generating one if not provided</returns>
        internal static Tuple<string, string> HashPassword(string password, string salt = null)
        {
            if (!String.IsNullOrEmpty(salt))
            {
                return Tuple.Create(ComputeHash(password, salt), "");
            }
            else
            {
                string generatedSalt = GenerateSalt();
                return Tuple.Create(ComputeHash(password, generatedSalt), generatedSalt);
            }
        }

        /// <summary>
        /// Used to hash the password provided by the HashPassword method
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns>A hashed string for the password provided</returns>
        static string ComputeHash(string password, string salt)
        {
            StringBuilder sb = new StringBuilder();
            var enc = System.Text.Encoding.Default;
            Byte[] bytes;

            using (SHA256 newHash = SHA256.Create())
            {
                bytes = newHash.ComputeHash(enc.GetBytes(DatabaseProperties.DBSecret + password + salt));

                foreach (Byte b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }
            }

            return sb.ToString();
        }
    }
}
