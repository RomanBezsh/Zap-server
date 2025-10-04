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

        // ID автора комментария
        public int UserId { get; set; }

        // Имя или ник автора (если нужно отобразить)
        public string AuthorName { get; set; } = string.Empty;

        // ID поста, к которому относится комментарий
        public int PostId { get; set; }

        // Текст комментария
        public string Content { get; set; } = string.Empty;

        // Дата публикации
        public DateTime CreatedAt { get; set; }

        // Количество лайков
        public int LikesCount { get; set; }
    }

}
