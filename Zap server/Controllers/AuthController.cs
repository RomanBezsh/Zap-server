using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Zap.BLL.DTO;
using Zap.BLL.Interfaces;
using Zap.DAL.Entities;

namespace Zap_server.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IUserService _userService;
        private IPasswordService _passwordService;
        private IAuthService _authService;
        public AuthController(IUserService userService, IPasswordService passwordService, IAuthService authService)
        {
            _userService = userService;
            _passwordService = passwordService;
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            UserDTO userDTO = new UserDTO
            {
                Username = registerRequestDTO.Username,
                Email = registerRequestDTO.Email,
                PasswordHash = _passwordService.HashPassword(registerRequestDTO.Password),
                DisplayName = registerRequestDTO.DisplayName,
                DateOfBirth = registerRequestDTO.DateOfBirth,
                PhoneNumber = registerRequestDTO.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
            };
            await _userService.CreateUser(userDTO);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string token = await _authService.AuthenticateAndGenerateTokenAsync(loginRequestDTO.UsernameOrEmail, loginRequestDTO.Password);
            return Ok(new
            {
                Token = token,
                Message = "Successs"
            });
        }
    }
}
