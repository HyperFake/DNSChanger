using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DNS_changer.Helper
{
    public class PasswordHelper
    {
        public string HashPassword(string password)
        {
            byte[] data = Encoding.ASCII.GetBytes(password);
            data = new SHA256Managed().ComputeHash(data);
            string hash = Encoding.ASCII.GetString(data);

            return hash;
        }

        public bool ComparePasswords(string inputPassword, string storedPassword)
        {
            if (HashPassword(inputPassword).Equals(HashPassword(storedPassword)))
                return true;
            else
                return false;

        }

        public bool ComparePasswordToStored(string newPassword)
        {
            if (HashPassword(newPassword).Equals(Properties.Settings.Default.Password))
                return true;
            else
                return false;
        }
    }

}
