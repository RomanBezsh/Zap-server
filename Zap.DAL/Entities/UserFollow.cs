using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zap.DAL.Entities
{
    public class UserFollow
    {
        // composite PK: (FollowerId, FollowedId)
        public int FollowerId { get; set; }   // кто подписан
        public User? Follower { get; set; }

        public int FollowedId { get; set; }   // на кого подписаны
        public User? Followed { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
