using System;
using System.Collections.Generic;

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

        // === Новое ===
        public int? ParentCommentId { get; set; } // ссылка на родительский комментарий
        public List<CommentDTO> Replies { get; set; } = new(); // ответы на комментарий

        public int LikesCount { get; set; }
        public bool IsLikedByCurrentUser { get; set; }

        public List<MediaAttachmentDTO> Attachments { get; set; } = new();
    }
}
