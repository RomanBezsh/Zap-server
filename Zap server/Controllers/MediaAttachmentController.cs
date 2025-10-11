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

        [HttpPost]
        public async Task<ActionResult> CreateMediaAttachment([FromBody] MediaAttachmentDTO mediaAttachmentDTO)
        {
            await _mediaAttachmentService.CreateMediaAttachment(mediaAttachmentDTO);
            return Ok();
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
