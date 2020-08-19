using System.Text;
using System.Security.Cryptography;

namespace DNS_changer.Helper
{
    public static class PasswordHelper
    {
        /// <summary>
        /// Hashes password string
        /// </summary>
        /// <param name="password">Password string</param>
        /// <returns>hashed password string</returns>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return "";
            byte[] data = Encoding.ASCII.GetBytes(password);
            data = new SHA256Managed().ComputeHash(data);
            string hash = Encoding.ASCII.GetString(data);

            return hash;
        }

        /// <summary>
        /// Compares two password hashes
        /// </summary>
        /// <param name="firstPass">First password</param>
        /// <param name="secondPass">Second password</param>
        /// <returns>true if equals, false otherwise</returns>
        public static bool ComparePasswords(string firstPass, string secondPass)
        {
            if (string.IsNullOrWhiteSpace(firstPass) || string.IsNullOrWhiteSpace(secondPass)) return false;
            if (HashPassword(firstPass).Equals(HashPassword(secondPass)))
                return true;
            else
                return false;

        }

        /// <summary>
        /// Compares password string to saved password
        /// </summary>
        /// <param name="newPassword">Password string</param>
        /// <returns>true is equals, false otherwise</returns>
        public static bool ComparePasswordToStored(string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword)) return false;
            if (HashPassword(newPassword).Equals(Properties.Settings.Default.Password))
                return true;
            else
                return false;
        }
    }

}
