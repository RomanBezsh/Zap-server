using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zap.DAL.Entities
{
    public class CommentLike : LikeBase
    {
        public int CommentId { get; set; }
        public Comment? Comment { get; set; }
    }
}
