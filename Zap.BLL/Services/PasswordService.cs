using System;
using System.Text;
using System.Threading.Tasks;
using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using Zap.BLL.Interfaces;

namespace Zap.BLL.Services
{
    public class PasswordService : IPasswordService
    {
        private const int MemoryKb = 65536; 
        private const int Iterations = 3;
        private const int DegreeOfParallelism = 2;

        public string HashPassword(string password)
        {
            // generate salt
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            using var argon = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = DegreeOfParallelism,
                Iterations = Iterations,
                MemorySize = MemoryKb
            };
            var hash = argon.GetBytes(32);
            // encode params + salt + hash, e.g. "$argon2id$v=19$m=65536,t=3,p=2$<salt>$<hash>"
            var sSalt = Convert.ToBase64String(salt);
            var sHash = Convert.ToBase64String(hash);
            return $"m={MemoryKb},t={Iterations},p={DegreeOfParallelism}.{sSalt}.{sHash}";
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var parts = hashedPassword.Split('.');
            if (parts.Length != 3) return false;
            // parse params from parts[0] if needed
            var salt = Convert.FromBase64String(parts[1]);
            var storedHash = Convert.FromBase64String(parts[2]);

            using var argon = new Argon2id(Encoding.UTF8.GetBytes(providedPassword))
            {
                Salt = salt,
                DegreeOfParallelism = DegreeOfParallelism,
                Iterations = Iterations,
                MemorySize = MemoryKb
            };
            var computed = argon.GetBytes(storedHash.Length);
            return CryptographicOperations.FixedTimeEquals(storedHash, computed);
        }
    }
}
