using Microsoft.AspNetCore.Mvc;
using Zap.BLL.DTO;
using Zap.BLL.Interfaces;

namespace Zap_server.Controllers
{
    [ApiController]
    [Route("api/Сomments")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // Получить все комментарии
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetComments()
        {
            var comments = await _commentService.GetAllComments();
            return Ok(comments);
        }

        // Получить комментарий по Id
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDTO?>> GetComment(int id)
        {
            var comment = await _commentService.GetCommentById(id);
            if (comment == null)
                return NotFound();
            return Ok(comment);
        }

        // Получить комментарии конкретного поста
        [HttpGet("post/{postId}")]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetCommentsForPost(int postId)
        {
            var comments = await _commentService.GetAllCommentsForPost(postId);
            if (!comments.Any())
                return NotFound($"Комментарии для поста с Id {postId} не найдены.");
            return Ok(comments);
        }

        // Создать новый комментарий
        [HttpPost]
        public async Task<ActionResult> CreateComment([FromBody] CommentDTO commentDTO)
        {
            await _commentService.CreateComment(commentDTO);
            return Ok();
        }

        // Обновить комментарий
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateComment(int id, [FromBody] CommentDTO commentDTO)
        {
            if (id != commentDTO.Id)
                return BadRequest("Id комментария не совпадает.");

            await _commentService.UpdateComment(commentDTO);
            return Ok();
        }

        // Удалить комментарий
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComment(int id)
        {
            await _commentService.DeleteComment(id);
            return Ok();
        }
    }
}
