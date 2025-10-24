using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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

        // ⚠️ Не маппим эти поля на БД
        [NotMapped]
        public string? VerificationCode { get; set; }

        [NotMapped]
        public DateTime? CodeExpiration { get; set; }

        public ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();
        public ICollection<CommentLike> CommentLikes { get; set; } = new List<CommentLike>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<UserFollow> Followers { get; set; } = new List<UserFollow>();
        public ICollection<UserFollow> Following { get; set; } = new List<UserFollow>();
    }
}