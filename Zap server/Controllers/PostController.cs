using Microsoft.AspNetCore.Mvc;
using Zap.BLL.DTO;
using Zap.BLL.Interfaces;
using Zap.BLL.Services;

namespace Zap_server.Controllers
{
    [ApiController]
    [Route("api/Posts")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IWebHostEnvironment _env;
        private readonly IMediaAttachmentService _mediaAttachmentService;

        public PostController(IPostService postService, IWebHostEnvironment env, IMediaAttachmentService mediaAttachmentService)
        {
            _postService = postService;
            _env = env;
            _mediaAttachmentService = mediaAttachmentService;
        }

        [HttpGet]
        public async Task<IEnumerable<PostDTO>> GetPosts()
        {
            return await _postService.GetAllPostsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDTO?>> GetPost(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            if (post == null)
                return NotFound();
            return Ok(post);
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPostsByUser(int userId)
        {
            var posts = await _postService.GetPostsByUserAsync(userId);
            return Ok(posts);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreatePost([FromForm] CreatePostRequestDTO request)
        {
            Console.WriteLine($"📩 CreatePost: Content = {request.Content}");

            var post = new PostDTO
            {
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
                UserId = 1 // временно
            };

            await _postService.CreatePostAsync(post);

            if (request.File != null && request.File.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "media");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(request.File.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using var fs = new FileStream(filePath, FileMode.Create);
                await request.File.CopyToAsync(fs);

                var attachment = new MediaAttachmentDTO
                {
                    Url = $"/media/{uniqueFileName}",
                    FileName = request.File.FileName,
                    ContentType = request.File.ContentType,
                    UploadedAt = DateTime.UtcNow,
                    PostId = post.Id
                };

                await _mediaAttachmentService.CreateMediaAttachmentAsync(attachment);
            }

            return Ok(new { Message = "Пост создан" });
        }

        [HttpPost("{id}/attachments")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UploadAttachment(int id, IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("No file");

            var uploadsFolder = Path.Combine(_env.WebRootPath, "media");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using var fs = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fs);

            var attachment = new MediaAttachmentDTO
            {
                Url = $"/media/{uniqueFileName}",
                MediaType = "gif",
                ContentType = file.ContentType,
                FileName = file.FileName,
                FileSize = file.Length,
                UploadedAt = DateTime.UtcNow,
                CommentId = id 
            };

            await _mediaAttachmentService.CreateMediaAttachmentAsync(attachment);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] PostDTO postDTO)
        {
            if (id != postDTO.Id)
                return BadRequest();
            await _postService.UpdatePostAsync(postDTO);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _postService.DeletePostAsync(id);
            return Ok();
        }

    }
}
