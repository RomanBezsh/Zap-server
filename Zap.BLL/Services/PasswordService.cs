using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
using Zap.BLL.Interfaces;


namespace Zap.BLL.Services
{
    public class PasswordService : IPasswordService
    {
        private const int MemoryKb = 65536; // 64 MB (значение по умолчанию при создании новых хешей)
        private const int Iterations = 3;   // значение по умолчанию
        private const int DegreeOfParallelism = 2; // значение по умолчанию

        public string HashPassword(string password)
        {
            // Генерация соли
            byte[] salt = RandomNumberGenerator.GetBytes(16);

            using var argon = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = DegreeOfParallelism,
                Iterations = Iterations,
                MemorySize = MemoryKb
            };

            byte[] hash = argon.GetBytes(32);

            string sSalt = Convert.ToBase64String(salt);
            string sHash = Convert.ToBase64String(hash);

            // Формат хранения: параметры.ArgonSalt.ArgonHash
            return $"m={MemoryKb},t={Iterations},p={DegreeOfParallelism}.{sSalt}.{sHash}";
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(hashedPassword))
                    return false;

                // Обрезаем случайные пробелы
                hashedPassword = hashedPassword.Trim();

                var parts = hashedPassword.Split('.');
                if (parts.Length != 3)
                    return false;

                // Разбор параметров из parts[0] (пример: "m=65536,t=3,p=2")
                int memoryKb = MemoryKb;
                int iterations = Iterations;
                int degree = DegreeOfParallelism;

                var paramPart = parts[0];
                var paramPairs = paramPart.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var pair in paramPairs)
                {
                    var kv = pair.Split('=', 2, StringSplitOptions.RemoveEmptyEntries);
                    if (kv.Length != 2) continue;
                    var key = kv[0].Trim().ToLowerInvariant();
                    var val = kv[1].Trim();
                    if (key == "m" && int.TryParse(val, out var mv)) memoryKb = mv;
                    else if (key == "t" && int.TryParse(val, out var tv)) iterations = tv;
                    else if (key == "p" && int.TryParse(val, out var pv)) degree = pv;
                }

                var salt = Convert.FromBase64String(parts[1]);
                var storedHash = Convert.FromBase64String(parts[2]);

                using var argon = new Argon2id(Encoding.UTF8.GetBytes(password))
                {
                    Salt = salt,
                    DegreeOfParallelism = degree,
                    Iterations = iterations,
                    MemorySize = memoryKb
                };

                var computed = argon.GetBytes(storedHash.Length);

                // Сравнение в постоянное время (без утечки времени)
                return CryptographicOperations.FixedTimeEquals(storedHash, computed);
            }
            catch
            {
                return false;
            }
        }
    }
}