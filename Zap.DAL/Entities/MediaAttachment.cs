using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zap.DAL.Entities
{
    public class MediaAttachment
    {
        public int Id { get; set; }

        // Тип вложения: image, video, audio, etc.
        public string MediaType { get; set; } 

        // Путь или URL к файлу
        public string Url { get; set; } 

        // Название файла (опционально)
        public string FileName { get; set; } 

        // Размер файла в байтах
        public long FileSize { get; set; }

        // MIME-тип (например, image/jpeg)
        public string ContentType { get; set; } 

        // Дата загрузки
        public DateTime UploadedAt { get; set; } 

        // Связь с постом
        public int PostId { get; set; }
        public Post? Post { get; set; }
    }

}
