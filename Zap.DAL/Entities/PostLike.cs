using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zap.DAL.Entities
{
    public class PostLike : LikeBase
    {
        public int PostId { get; set; }
        public Post? Post { get; set; }
    }
}
