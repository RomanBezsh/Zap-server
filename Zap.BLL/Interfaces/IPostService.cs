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
        Task CreatePostAsync(PostDTO postDTO);
        Task UpdatePostAsync(PostDTO postDTO);
        Task DeletePostAsync(int id);
        Task<IEnumerable<PostDTO>> GetAllPostsAsync();
        Task<PostDTO?> GetPostByIdAsync(int id);
        Task<IEnumerable<PostDTO>> GetPostsByUserAsync(int userId);

    }
}
