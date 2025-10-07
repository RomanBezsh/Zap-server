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
        Task CreateMediaAttachment(MediaAttachmentDTO mediaAttachmentDTO);
        Task UpdateMediaAttachment(MediaAttachmentDTO mediaAttachmentDTO);
        Task DeleteMediaAttachment(int id);
        Task<IEnumerable<MediaAttachmentDTO>> GetAllMediaAttachments();
    }
}
