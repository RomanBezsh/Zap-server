using System;
using System.Collections.Generic;

namespace Zap.DAL.Entities
{
    public class Post
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? Author { get; set; }

        public string? Content { get; set; }

        public DateTime CreatedAt { get; set; } 

        public int LikesCount { get; set; }

        public int RepostsCount { get; set; }

        public int? ReplyToPostId { get; set; }
        public Post? ReplyToPost { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<MediaAttachment> Attachments { get; set; } = new List<MediaAttachment>();
        public ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();
    }
}
