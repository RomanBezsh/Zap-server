using Microsoft.AspNetCore.Mvc;
using Zap.BLL.DTO;
using Zap.BLL.Interfaces;

namespace Zap_server.Controllers
{
    [Route("api/MediaAttachments")]
    [ApiController]
    public class MediaAttachmentController : ControllerBase
    {
        private readonly IMediaAttachmentService _mediaAttachmentService;
        private readonly IWebHostEnvironment _env;

        public MediaAttachmentController(IMediaAttachmentService mediaAttachmentService, IWebHostEnvironment env)
        {
            _mediaAttachmentService = mediaAttachmentService;
            _env = env;
        }

        [HttpGet]
        public async Task<IEnumerable<MediaAttachmentDTO>> GetMediaAttachments()
        {
            return await _mediaAttachmentService.GetAllMediaAttachments();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MediaAttachmentDTO?>> GetMediaAttachment(int id)
        {
            var mediaAttachment = await _mediaAttachmentService.GetMediaAttachmentById(id);
            if (mediaAttachment == null)
                return NotFound();
            return Ok(mediaAttachment);
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UploadMedia([FromForm] UploadMediaRequestDTO request)
        {
            var file = request?.File;
            if (file == null || file.Length == 0)
                return BadRequest("Файл не выбран");

            var uploadsFolder = Path.Combine(_env.ContentRootPath, "media");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }

            var mediaType = file.ContentType.StartsWith("video")
                ? "video"
                : file.ContentType.StartsWith("image")
                    ? "image"
                    : "other";

            var attachment = new MediaAttachmentDTO
            {
                MediaType = mediaType,
                Url = $"/media/{uniqueFileName}",
                FileName = file.FileName,
                FileSize = file.Length,
                ContentType = file.ContentType,
                UploadedAt = DateTime.UtcNow,
                PostId = request.PostId,
                CommentId = request.CommentId
            };

            await _mediaAttachmentService.CreateMediaAttachment(attachment);

            return Ok(new { Url = attachment.Url, Type = attachment.MediaType });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMediaAttachment(int id, [FromBody] MediaAttachmentDTO mediaAttachmentDTO)
        {
            if (id != mediaAttachmentDTO.Id)
                return BadRequest();
            await _mediaAttachmentService.UpdateMediaAttachment(mediaAttachmentDTO);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMediaAttachment(int id)
        {
            await _mediaAttachmentService.DeleteMediaAttachment(id);
            return Ok();
        }
    }
}