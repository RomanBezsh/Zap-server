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
        public int? ParentCommentId { get; set; }  // null → корневой комментарий
        public Comment? ParentComment { get; set; } // ссылка на родителя
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();

        // Вложения (GIF/изображения) привязанные к комментарию
        public ICollection<MediaAttachment> Attachments { get; set; } = new List<MediaAttachment>();
        public ICollection<CommentLike> CommentLikes { get; set; } = new List<CommentLike>();
    }
}