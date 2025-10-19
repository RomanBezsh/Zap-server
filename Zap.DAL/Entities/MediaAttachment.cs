using System;

namespace Zap.DAL.Entities
{
    public class MediaAttachment
    {
        public int Id { get; set; }

        public string MediaType { get; set; } = null!; // image, gif, video, etc.
        public string Url { get; set; } = null!;
        public string? FileName { get; set; }
        public long FileSize { get; set; }
        public string ContentType { get; set; } = null!; // e.g. image/gif
        public DateTime UploadedAt { get; set; }

        // Связь с постом (если вложение к посту)
        public int? PostId { get; set; }
        public Post? Post { get; set; }

        // Связь с комментарием (если вложение к комментарию)
        public int? CommentId { get; set; }
        public Comment? Comment { get; set; }
    }
}
