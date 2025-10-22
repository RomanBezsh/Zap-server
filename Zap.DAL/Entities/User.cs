using System;
using System.Collections.Generic;

namespace Zap.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? DisplayName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? Bio { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        public bool IsSuspended { get; set; } = false;

        public ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();
        public ICollection<CommentLike> CommentLikes { get; set; } = new List<CommentLike>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        // Navigation collections for explicit join entity:
        // Followers: users who follow THIS user (FollowedId == this.Id)
        public ICollection<UserFollow> Followers { get; set; } = new List<UserFollow>();

        // Following: users THIS user follows (FollowerId == this.Id)
        public ICollection<UserFollow> Following { get; set; } = new List<UserFollow>();
    }
}
