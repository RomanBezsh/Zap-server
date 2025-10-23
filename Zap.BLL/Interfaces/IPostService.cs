using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zap.BLL.DTO;

namespace Zap.BLL.Interfaces
{
    public interface IPostService
    {
        Task CreatePost(PostDTO postDTO);
        Task UpdatePost(PostDTO postDTO);
        Task DeletePost(int id);
        Task<IEnumerable<PostDTO>> GetAllPosts();
        Task<PostDTO?> GetPostById(int id);
        Task<IEnumerable<PostDTO>> GetPostsByUserAsync(int userId);

    }
}
