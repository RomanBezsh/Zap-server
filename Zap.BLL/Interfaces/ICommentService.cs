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
        Task CreateComment(CommentDTO postDTO);
        Task UpdateComment(CommentDTO postDTO);
        Task DeleteComment(int id);
        Task<CommentDTO?> GetCommentById(int id);
        Task<IEnumerable<CommentDTO>> GetAllComments();
    }
}
