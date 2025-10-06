using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Zap.BLL.DTO;
using Zap.BLL.Interfaces;

namespace Zap_server.Controllers
{
    [ApiController]
    [Route("api/Users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            return await _userService.GetUsers();
        }
    }
}
