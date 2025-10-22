using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zap.BLL.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string? AuthorUsername { get; set; } 

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public int LikesCount { get; set; }

        public int RepostsCount { get; set; }

        public int? ReplyToPostId { get; set; }

        public List<CommentDTO> Comments { get; set; } = new();

        public List<MediaAttachmentDTO> Attachments { get; set; } = new();

        public bool IsLikedByCurrentUser { get; set; }
    }
}
