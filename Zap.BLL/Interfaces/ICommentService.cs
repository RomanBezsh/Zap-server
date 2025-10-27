using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zap.BLL.DTO;

namespace Zap.BLL.Interfaces
{
    public interface ICommentService
    {
        Task CreateCommentAsync(CommentDTO commentDTO);
        Task UpdateCommentAsync(CommentDTO commentDTO);
        Task DeleteCommentAsync(int id);
        Task<CommentDTO?> GetCommentByIdAsync(int id);
        Task<IEnumerable<CommentDTO>> GetAllCommentsAsync();
        Task<IEnumerable<CommentDTO>> GetAllCommentsForPostAsync(int postId);

    }
}
