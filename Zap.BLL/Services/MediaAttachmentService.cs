using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zap.BLL.DTO;
using Zap.BLL.Interfaces;
using Zap.DAL.Entities;
using Zap.DAL.Interfaces;
using AutoMapper;
namespace Zap.BLL.Services
{
    public class MediaAttachmentService : IMediaAttachmentService
    {
        IUnitOfWork Database { get; set; }

        public MediaAttachmentService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task CreateMediaAttachment(MediaAttachmentDTO mediaAttachmentDTO)
        {
            MediaAttachment mediaAttachment = new MediaAttachment
            {
                MediaType = mediaAttachmentDTO.MediaType,
                Url = mediaAttachmentDTO.Url,
                FileName = mediaAttachmentDTO.FileName,
                FileSize = mediaAttachmentDTO.FileSize,
                ContentType = mediaAttachmentDTO.ContentType,
                UploadedAt = mediaAttachmentDTO.UploadedAt,
                PostId = mediaAttachmentDTO.PostId
            };
            await Database.MediaAttachments.AddAsync(mediaAttachment);
            await Database.SaveAsync();
        }
        public async Task UpdateMediaAttachment(MediaAttachmentDTO mediaAttachmentDTO)
        {
            var mediaAttachment = await Database.MediaAttachments.GetByIdAsync(mediaAttachmentDTO.Id);
            if (mediaAttachment != null)
            {
                mediaAttachment.MediaType = mediaAttachmentDTO.MediaType;
                mediaAttachment.Url = mediaAttachmentDTO.Url;
                mediaAttachment.FileName = mediaAttachmentDTO.FileName;
                mediaAttachment.FileSize = mediaAttachmentDTO.FileSize;
                mediaAttachment.ContentType = mediaAttachmentDTO.ContentType;
                mediaAttachment.UploadedAt = mediaAttachmentDTO.UploadedAt;
                mediaAttachment.PostId = mediaAttachmentDTO.PostId;
                Database.MediaAttachments.Update(mediaAttachment);
                await Database.SaveAsync();
            }
        }
        public async Task DeleteMediaAttachment(int id)
        {
            var mediaAttachment = await Database.MediaAttachments.GetByIdAsync(id);
            if (mediaAttachment != null)
            {
                Database.MediaAttachments.Delete(mediaAttachment);
                await Database.SaveAsync();
            }
        }
        public async Task<MediaAttachmentDTO?> GetMediaAttachmentById(int id)
        {
            var mediaAttachment = await Database.MediaAttachments.GetByIdAsync(id);
            if (mediaAttachment == null)
                return null;
            return new MediaAttachmentDTO
            {
                Id = mediaAttachment.Id,
                MediaType = mediaAttachment.MediaType,
                Url = mediaAttachment.Url,
                FileName = mediaAttachment.FileName,
                FileSize = mediaAttachment.FileSize,
                ContentType = mediaAttachment.ContentType,
                UploadedAt = mediaAttachment.UploadedAt,
                PostId = mediaAttachment.PostId
            };
        }
        public async Task<IEnumerable<MediaAttachmentDTO>> GetAllMediaAttachments()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<MediaAttachment, MediaAttachmentDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<MediaAttachment>, IEnumerable<MediaAttachmentDTO>>(await Database.MediaAttachments.GetAllAsync());
        }

    }
}
