using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Isopoh.Cryptography.Argon2;
using Isopoh.Cryptography.SecureArray;
using Zap.BLL.Interfaces;

namespace Zap.BLL.Services
{
    public class PasswordService : IPasswordService
    {
        public string HashPassword(string password)
        {
            var hash = Argon2.Hash(password);
            return hash;
        }
        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            return Argon2.Verify(hashedPassword, providedPassword);
        }
    }
}
