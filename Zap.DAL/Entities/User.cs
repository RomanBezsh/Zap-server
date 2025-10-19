using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        // Fixed initializer (was "[]", which is invalid)
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }

}
