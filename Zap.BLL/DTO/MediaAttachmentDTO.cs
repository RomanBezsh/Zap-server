using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zap.BLL.DTO
{
    public class MediaAttachmentDTO
    {
        public int Id { get; set; }

        public string MediaType { get; set; }

        public string Url { get; set; }

        public string? FileName { get; set; }

        public long FileSize { get; set; }

        public string ContentType { get; set; }

        public int? PostId { get; set; }

        public int? CommentId { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}
