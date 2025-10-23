using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zap.BLL.DTO
{
    public class UploadMediaRequestDTO
    {
        [Required]
        public IFormFile File { get; set; } = default!;

        public int? PostId { get; set; }

        public int? CommentId { get; set; }
    }
}
