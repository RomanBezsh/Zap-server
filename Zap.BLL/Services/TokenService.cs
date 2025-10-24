using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Zap.BLL.DTO;
using Zap.BLL.Interfaces;


namespace Zap.BLL.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(UserDTO user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            // Защита от отсутствующей конфигурации
            var keyString = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];

            if (string.IsNullOrWhiteSpace(keyString))
                throw new InvalidOperationException("JWT key is not configured. Set Jwt:Key in configuration (appsettings, environment or user-secrets).");
            if (string.IsNullOrWhiteSpace(issuer))
                throw new InvalidOperationException("JWT issuer is not configured. Set Jwt:Issuer in configuration.");
            if (string.IsNullOrWhiteSpace(audience))
                throw new InvalidOperationException("JWT audience is not configured. Set Jwt:Audience in configuration.");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim("username", user.Username ?? string.Empty)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}