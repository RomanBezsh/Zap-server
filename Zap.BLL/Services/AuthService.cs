using Microsoft.Identity.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Zap.BLL.Interfaces;
using Zap.DAL.Repositories;

namespace Zap.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        private readonly ConcurrentDictionary<string, (string Code, DateTime Expiration)> _verificationCodes
        = new ConcurrentDictionary<string, (string, DateTime)>();
        private readonly IEmailService _emailService;

        public AuthService(IUserService userService, IPasswordService passwordService, ITokenService tokenService, IEmailService emailService)
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

            bool isValidPassword = _passwordService.VerifyPassword(password, user.PasswordHash);
            if (!isValidPassword)
                throw new UnauthorizedAccessException("Неверный пароль");

            string token = _tokenService.GenerateToken(user);

            return token;
        }
        public async Task<string> GenerateVerificationCodeAsync(string email)
        {
            var code = new Random().Next(100000, 999999).ToString(); 
            var expiration = DateTime.UtcNow.AddMinutes(15);
            _verificationCodes[email] = (code, expiration);

            await _emailService.SendVerificationCodeAsync(email, code);

            return code;
        }
        public async Task<bool> VerifyCodeAsync(string email, string code)
        {
            if (_verificationCodes.TryGetValue(email, out var entry))
            {
                if (entry.Code == code && entry.Expiration > DateTime.UtcNow)
                {
                    _verificationCodes.TryRemove(email, out _); 
                    return true;
                }
            }
            return false;
        }
    }
}
