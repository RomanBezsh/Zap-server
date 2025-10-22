using Microsoft.AspNetCore.Mvc;
using Zap.BLL.Interfaces;

namespace Zap_server.Controllers
{
    [ApiController]
    [Route("api/Likes")]
    public class LikesController : ControllerBase
    {
        private readonly ILikeService _likeService;
        public LikesController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        // ========================= POST LIKES =========================
        [HttpPost("posts/{postId}/toggle/{userId}")]
        public async Task<IActionResult> TogglePostLike(int postId, int userId)
        {
            await _likeService.TogglePostLikeAsync(postId, userId);
            return Ok(new { message = "Toggled post like successfully." });
        }

        [HttpGet("post/{postId}/count")]
        public async Task<IActionResult> GetPostLikesCount(int postId)
        {
            var count = await _likeService.GetPostLikesCountAsync(postId);
            return Ok(new { postId, likesCount = count });
        }

        [HttpGet("post/{postId}/is-liked/{userId}")]
        public async Task<IActionResult> IsPostLikedByUser(int postId, int userId)
        {
            var isLiked = await _likeService.IsPostLikedByUserAsync(postId, userId);
            return Ok(new { postId, userId, isLiked });
        }
        [HttpGet("post/all")]
        public async Task<IActionResult> GetAllPostLikes()
        {
            var likes = await _likeService.GetAllPostLikesAsync();
            return Ok(likes);
        }

        // ========================= COMMENT LIKES =========================

        [HttpPost("comment/{commentId}/toggle/{userId}")]
        public async Task<IActionResult> ToggleCommentLike(int commentId, int userId)
        {
            await _likeService.ToggleCommentLikeAsync(commentId, userId);
            return Ok(new { message = "Toggled comment like successfully." });
        }

        [HttpGet("comment/{commentId}/count")]
        public async Task<IActionResult> GetCommentLikesCount(int commentId)
        {
            var count = await _likeService.GetCommentLikesCountAsync(commentId);
            return Ok(new { commentId, likesCount = count });
        }

        [HttpGet("comment/{commentId}/is-liked/{userId}")]
        public async Task<IActionResult> IsCommentLikedByUser(int commentId, int userId)
        {
            var isLiked = await _likeService.IsCommentLikedByUserAsync(commentId, userId);
            return Ok(new { commentId, userId, isLiked });
        }

        [HttpGet("comment/all")]
        public async Task<IActionResult> GetAllCommentLikes()
        {
            var likes = await _likeService.GetAllCommentLikesAsync();
            return Ok(likes);
        }




    }
}
