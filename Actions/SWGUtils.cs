using System;
using System.Text;
using System.Security.Cryptography;

namespace LauncherAPI.Controllers
{
    public class SWGUtils
    {
        public int GenerateStationId()
        {
            var rand = new Random();
            return rand.Next(1000000000, 2147483647);
        }

        public string GenerateSalt(int maxSalt = 32)
        {
            Byte[] bytes = new byte[maxSalt];

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

        public string HashPassword(string password, string dbSecret, string salt = null)
        {
            if (!String.IsNullOrEmpty(salt))
            {
                return ComputeHash(password, dbSecret, salt);
            }
            else
            {
                return ComputeHash(password, dbSecret, GenerateSalt());
            }
        }

        string ComputeHash(string password, string dbSecret, string salt)
        {
            StringBuilder sb = new StringBuilder();
            var enc = System.Text.Encoding.Default;
            Byte[] bytes;

            using (SHA256 newHash = SHA256.Create())
            {
                bytes = newHash.ComputeHash(enc.GetBytes(dbSecret + password + salt));

                foreach (Byte b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }
            }

            return sb.ToString();
        }
    }
}
