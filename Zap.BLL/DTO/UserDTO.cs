using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zap.BLL.DTO
{
    public class UserDTO
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
        public bool IsEmailVerified { get; set; } 
        public bool IsSuspended { get; set; } 
    }
}
