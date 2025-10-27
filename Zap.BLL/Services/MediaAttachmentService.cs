using AutoMapper;
using Zap.BLL.DTO;
using Zap.BLL.Interfaces;
using Zap.DAL.Entities;
using Zap.DAL.Interfaces;

namespace Zap.BLL.Services
{
    public class MediaAttachmentService : IMediaAttachmentService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public MediaAttachmentService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow ?? throw new ArgumentNullException(nameof(uow));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task CreateMediaAttachmentAsync(MediaAttachmentDTO mediaAttachmentDTO)
        {
            if (mediaAttachmentDTO == null) throw new ArgumentNullException(nameof(mediaAttachmentDTO));

            var mediaAttachment = _mapper.Map<MediaAttachment>(mediaAttachmentDTO);
            if (mediaAttachment.UploadedAt == default) mediaAttachment.UploadedAt = DateTime.UtcNow;

            await _db.MediaAttachments.AddAsync(mediaAttachment);
            await _db.SaveAsync();
        }

        public async Task UpdateMediaAttachmentAsync(MediaAttachmentDTO mediaAttachmentDTO)
        {
            if (mediaAttachmentDTO == null) throw new ArgumentNullException(nameof(mediaAttachmentDTO));

            var mediaAttachment = await _db.MediaAttachments.GetByIdAsync(mediaAttachmentDTO.Id);
            if (mediaAttachment != null)
            {
                _mapper.Map(mediaAttachmentDTO, mediaAttachment);
                _db.MediaAttachments.Update(mediaAttachment);
                await _db.SaveAsync();
            }
        }

        public async Task DeleteMediaAttachmentAsync(int id)
        {
            var mediaAttachment = await _db.MediaAttachments.GetByIdAsync(id);
            if (mediaAttachment != null)
            {
                _db.MediaAttachments.Delete(mediaAttachment);
                await _db.SaveAsync();
            }
        }

        public async Task<MediaAttachmentDTO?> GetMediaAttachmentByIdAsync(int id)
        {
            var mediaAttachment = await _db.MediaAttachments.GetByIdAsync(id);
            if (mediaAttachment == null)
                return null;

            return _mapper.Map<MediaAttachmentDTO>(mediaAttachment);
        }

        public async Task<IEnumerable<MediaAttachmentDTO>> GetAllMediaAttachmentsAsync()
        {
            var attachments = await _db.MediaAttachments.GetAllAsync();
            return _mapper.Map<IEnumerable<MediaAttachmentDTO>>(attachments);
        }
    }
}