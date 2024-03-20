using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ParkingManagement.Utils
{
    public class BCryptConvertion
    {
        static string saltvalue = ConfigurationManager.AppSettings["SaltValue"];
        public static byte[] salt = Convert.FromBase64String(saltvalue);
        public static string Encrypt(string input)
        {
            // Hash the password using the salt
            string hashedPassword = HashPassword(input, salt);

            return hashedPassword;
        }

        public static bool Verify(string hashedvalue, string input)
        {
            // Hash the password using the salt
            string hashedPassword = HashPassword(input, salt);

            bool passwordsMatch = hashedvalue.Equals(hashedPassword);

            return passwordsMatch;
        }

        static string HashPassword(string password, byte[] salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Combine password and salt, then compute hash
                byte[] combinedBytes = Encoding.UTF8.GetBytes(password + Convert.ToBase64String(salt));
                byte[] hashedBytes = sha256.ComputeHash(combinedBytes);

                // Convert hashed bytes to string
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }


}