using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zap.BLL.DTO;

namespace Zap.BLL.Interfaces
{
    public interface ILikeService
    {
        Task TogglePostLikeAsync(int postId, int userId);
        Task<int> GetPostLikesCountAsync(int postId);
        Task<bool> IsPostLikedByUserAsync(int postId, int userId);

        Task ToggleCommentLikeAsync(int commentId, int userId);
        Task<int> GetCommentLikesCountAsync(int commentId);
        Task<bool> IsCommentLikedByUserAsync(int commentId, int userId);

        Task<IEnumerable<PostLikeDTO>> GetAllPostLikesAsync();
        Task<IEnumerable<CommentLikeDTO>> GetAllCommentLikesAsync();
    }

}
