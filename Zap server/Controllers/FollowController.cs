using Microsoft.AspNetCore.Mvc;
using Zap.BLL.DTO;
using Zap.BLL.Interfaces;

namespace Zap_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FollowController : ControllerBase
    {
        private readonly IUserService _userService;

        public FollowController(IUserService userService)
        {
            _userService = userService;
        }

        // ========================= FOLLOW / UNFOLLOW =========================

        [HttpPost("follow/{userId}/{targetUserId}")]
        public async Task<IActionResult> FollowUser(int userId, int targetUserId)
        {
            try
            {
                await _userService.FollowUser(userId, targetUserId);
                return Ok(new { message = $"User {userId} now follows {targetUserId}" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpPost("unfollow/{userId}/{targetUserId}")]
        public async Task<IActionResult> UnfollowUser(int userId, int targetUserId)
        {
            await _userService.UnfollowUser(userId, targetUserId);
            return Ok(new { message = $"User {userId} unfollowed {targetUserId}" });
        }

        // ========================= CHECK FOLLOWING =========================

        [HttpGet("{userId}/is-following/{targetUserId}")]
        public async Task<IActionResult> IsFollowing(int userId, int targetUserId)
        {
            var isFollowing = await _userService.IsFollowingAsync(userId, targetUserId);
            return Ok(new { userId, targetUserId, isFollowing });
        }

        // ========================= GET FOLLOWERS / FOLLOWING =========================

        [HttpGet("{userId}/followers")]
        public async Task<IActionResult> GetFollowers(int userId)
        {
            var followers = await _userService.GetFollowersAsync(userId); // возвращает IEnumerable<UserShortDTO>
            return Ok(followers);
        }

        [HttpGet("{userId}/following")]
        public async Task<IActionResult> GetFollowing(int userId)
        {
            var following = await _userService.GetFollowingAsync(userId); // возвращает IEnumerable<UserShortDTO>
            return Ok(following);
        }
    }
}
