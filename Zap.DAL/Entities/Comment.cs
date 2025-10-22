using System;
using System.Collections.Generic;

namespace Zap.DAL.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? Author { get; set; }
        public int PostId { get; set; }
        public Post? Post { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; } 
        public int LikesCount { get; set; }

        // Вложения (GIF/изображения) привязанные к комментарию
        public ICollection<MediaAttachment> Attachments { get; set; } = new List<MediaAttachment>();
        public ICollection<CommentLike> CommentLikes { get; set; } = new List<CommentLike>();
    }
}