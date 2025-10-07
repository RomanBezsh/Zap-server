using Microsoft.AspNetCore.Mvc;
using Zap.BLL.DTO;
using Zap.BLL.Interfaces;

namespace Zap_server.Controllers
{
    [ApiController]
    [Route("api/Posts")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IWebHostEnvironment _env;
        public PostController(IPostService postService, IWebHostEnvironment env)
        {
            _postService = postService;
            _env = env;
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
