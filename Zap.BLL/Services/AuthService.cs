using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zap.BLL.Interfaces;
using Zap.DAL.Entities;

namespace Zap.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;

        // Храним коды прямо в памяти, не в БД
        private static readonly Dictionary<string, (string Code, DateTime Expiration)> _codes = new();

        public AuthService(
            IUserService userService,
            IPasswordService passwordService,
            ITokenService tokenService,
            IEmailService emailService)
        {
            _userService = userService;
            _passwordService = passwordService;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        public async Task<string> AuthenticateAndGenerateTokenAsync(string usernameOrEmail, string password)
        {
            var user = await _userService.GetUserByUsernameOrEmail(usernameOrEmail);
            if (user == null)
                throw new UnauthorizedAccessException("Пользователь не найден");

            if (!_passwordService.VerifyPassword(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Неверный пароль");

            return _tokenService.GenerateToken(user);
        }

        // Генерация кода
        public async Task<string> GenerateVerificationCodeAsync(string email)
        {
            var user = await _userService.GetUserByUsernameOrEmail(email);
            if (user == null)
                throw new InvalidOperationException("Пользователь с таким email не найден");

            var code = new Random().Next(100000, 999999).ToString();
            var expiration = DateTime.UtcNow.AddMinutes(15);
            _codes[email] = (code, expiration);

            Console.WriteLine($"✅ Код для {email}: {code} (действует до {expiration})");

            await _emailService.SendVerificationCodeAsync(email, code);
            return code;
        }

        // Проверка кода
        public async Task<bool> VerifyCodeAsync(string email, string code)
        {
            if (_codes.TryGetValue(email, out var entry))
            {
                if (entry.Code == code && entry.Expiration > DateTime.UtcNow)
                {
                    _codes.Remove(email);
                    var user = await _userService.GetUserByUsernameOrEmail(email);
                    if (user != null)
                    {
                        user.IsEmailVerified = true;
                        await _userService.UpdateUser(user);
                    }
                    return true;
                }
            }
            return false;
        }
    }
}