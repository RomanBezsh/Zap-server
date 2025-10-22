using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Zap.BLL.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? AuthorName { get; set; } 

        public int PostId { get; set; }

        public string? Content { get; set; } 

        public DateTime CreatedAt { get; set; }

        public int LikesCount { get; set; }

        // Attachments для GIF/изображений/видео в комментариях
        public List<MediaAttachmentDTO> Attachments { get; set; } = new();
        public bool IsLikedByCurrentUser { get; set; }
    }
}
