using AutoMapper;
using Zap.BLL.DTO;
using Zap.BLL.Interfaces;
using Zap.DAL.Entities;
using Zap.DAL.Interfaces;

namespace Zap.BLL.Services
{
    public class MediaAttachmentService : IMediaAttachmentService
    {
        private readonly IUnitOfWork _database;
        private readonly IMapper _mapper;

        public MediaAttachmentService(IUnitOfWork uow, IMapper mapper)
        {
            _database = uow ?? throw new ArgumentNullException(nameof(uow));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task CreateMediaAttachment(MediaAttachmentDTO mediaAttachmentDTO)
        {
            if (mediaAttachmentDTO == null) throw new ArgumentNullException(nameof(mediaAttachmentDTO));

            var mediaAttachment = _mapper.Map<MediaAttachment>(mediaAttachmentDTO);
            if (mediaAttachment.UploadedAt == default) mediaAttachment.UploadedAt = DateTime.UtcNow;

            await _database.MediaAttachments.AddAsync(mediaAttachment);
            await _database.SaveAsync();
        }

        public async Task UpdateMediaAttachment(MediaAttachmentDTO mediaAttachmentDTO)
        {
            if (mediaAttachmentDTO == null) throw new ArgumentNullException(nameof(mediaAttachmentDTO));

            var mediaAttachment = await _database.MediaAttachments.GetByIdAsync(mediaAttachmentDTO.Id);
            if (mediaAttachment != null)
            {
                _mapper.Map(mediaAttachmentDTO, mediaAttachment);
                _database.MediaAttachments.Update(mediaAttachment);
                await _database.SaveAsync();
            }
        }

        public async Task DeleteMediaAttachment(int id)
        {
            var mediaAttachment = await _database.MediaAttachments.GetByIdAsync(id);
            if (mediaAttachment != null)
            {
                _database.MediaAttachments.Delete(mediaAttachment);
                await _database.SaveAsync();
            }
        }

        public async Task<MediaAttachmentDTO?> GetMediaAttachmentById(int id)
        {
            var mediaAttachment = await _database.MediaAttachments.GetByIdAsync(id);
            if (mediaAttachment == null)
                return null;

            return _mapper.Map<MediaAttachmentDTO>(mediaAttachment);
        }

        public async Task<IEnumerable<MediaAttachmentDTO>> GetAllMediaAttachments()
        {
            var attachments = await _database.MediaAttachments.GetAllAsync();
            return _mapper.Map<IEnumerable<MediaAttachmentDTO>>(attachments);
        }
    }
}