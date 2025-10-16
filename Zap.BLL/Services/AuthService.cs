using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public AuthService(IUserService userService, IPasswordService passwordService, ITokenService tokenService)
        {
            _userService = userService;
            _passwordService = passwordService;
            _tokenService = tokenService;
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

    }
}
