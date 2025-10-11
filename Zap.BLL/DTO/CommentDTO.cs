using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zap.BLL.DTO
{
    public class CommentDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string AuthorName { get; set; } 

        public int PostId { get; set; }

        public string Content { get; set; } 

        public DateTime CreatedAt { get; set; }

        public int LikesCount { get; set; }
    }

}
