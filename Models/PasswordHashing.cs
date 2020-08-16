using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DNS_changer.Models
{
    public class PasswordHashing
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
    }

}
