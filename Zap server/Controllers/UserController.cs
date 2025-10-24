using Microsoft.AspNetCore.Mvc;
using Zap.BLL.DTO;
using Zap.BLL.Interfaces;

namespace Zap_server.Controllers
{
    [ApiController]
    [Route("api/Users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _env;
        public UserController(IUserService userService, IWebHostEnvironment env)
        {
            _userService = userService;
            _env = env;
        }

        [HttpGet]
        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            return await _userService.GetAllUsers();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO?>> GetUser(int id)
        {
            var user = await _userService.GetUser(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> SearchUsers([FromQuery] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest("Username parameter is required");

            var users = await _userService.SearchUsersByUsername(username);
            return Ok(users);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> CreateUser([FromForm] UserDTO userDTO, IFormFile? profileImage)
        {
            if (profileImage != null)
            {
                var uploadsFolder = Path.Combine(_env.ContentRootPath, "media");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(profileImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await profileImage.CopyToAsync(fileStream);
                }

                userDTO.ProfileImageUrl = $"/media/{uniqueFileName}";
            }

            await _userService.CreateUser(userDTO);
            return Ok();
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UpdateUser(int id, [FromForm] UserDTO userDTO, IFormFile? profileImage)
        {
            if (id != userDTO.Id)
                return BadRequest("User ID mismatch");

            if (profileImage != null)
            {
                var uploadsFolder = Path.Combine(_env.ContentRootPath, "media");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(profileImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await profileImage.CopyToAsync(fileStream);
                }

                userDTO.ProfileImageUrl = $"/media/{uniqueFileName}";
            }

            await _userService.UpdateUser(userDTO);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUser(id);
            return Ok();
        }

        [HttpGet("{id}/followers")]
        public async Task<ActionResult<IEnumerable<UserShortDTO>>> GetFollowers(int id)
        {
            var followers = await _userService.GetFollowersAsync(id);
            return Ok(followers);
        }

        [HttpGet("{id}/following")]
        public async Task<ActionResult<IEnumerable<UserShortDTO>>> GetFollowing(int id)
        {
            var following = await _userService.GetFollowingAsync(id);
            return Ok(following);
        }
    }
}
