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
            return await _postService.GetAllPosts();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDTO?>> GetPost(int id)
        {
            var post = await _postService.GetPostById(id);
            if (post == null)
                return NotFound();
            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] PostDTO postDTO)
        {
            await _postService.CreatePost(postDTO);
            return Ok();
        }
        [HttpPost("{id}/attachments")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UploadAttachment(int id, IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("No file");

            var uploadsFolder = Path.Combine(_env.ContentRootPath, "media");
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

            await _mediaAttachmentService.CreateMediaAttachment(attachment);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] PostDTO postDTO)
        {
            if (id != postDTO.Id)
                return BadRequest();
            await _postService.UpdatePost(postDTO);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _postService.DeletePost(id);
            return Ok();
        }

    }
}
