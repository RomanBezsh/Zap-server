using Microsoft.AspNetCore.Mvc;
using Zap.BLL.DTO;
using Zap.BLL.Interfaces;

namespace Zap_server.Controllers
{
    [ApiController]
    [Route("api/Comments")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<IEnumerable<CommentDTO>> GetComments()
        {
            return await _commentService.GetAllComments();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDTO?>> GetComment(int id)
        {
            var comment = await _commentService.GetCommentById(id);
            if (comment == null)
                return NotFound();
            return Ok(comment);
        }

        [HttpPost]
        public async Task<ActionResult> CreateComment([FromBody] CommentDTO commentDTO)
        {
            await _commentService.CreateComment(commentDTO);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateComment(int id, [FromBody] CommentDTO commentDTO)
        {
            if (id != commentDTO.Id)
                return BadRequest();
            await _commentService.UpdateComment(commentDTO);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComment(int id)
        {
            await _commentService.DeleteComment(id);
            return Ok();
        }

    }
}
