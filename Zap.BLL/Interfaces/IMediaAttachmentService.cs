using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zap.BLL.DTO;

namespace Zap.BLL.Interfaces
{
    public interface IMediaAttachmentService
    {
        Task CreateMediaAttachmentAsync(MediaAttachmentDTO mediaAttachmentDTO);
        Task UpdateMediaAttachmentAsync(MediaAttachmentDTO mediaAttachmentDTO);
        Task DeleteMediaAttachmentAsync(int id);
        Task<MediaAttachmentDTO?> GetMediaAttachmentByIdAsync(int id);
        Task<IEnumerable<MediaAttachmentDTO>> GetAllMediaAttachmentsAsync();
    }
}
