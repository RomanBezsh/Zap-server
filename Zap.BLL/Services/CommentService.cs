using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zap.BLL.DTO;
using Zap.BLL.Interfaces;
using Zap.DAL.Entities;
using Zap.DAL.Interfaces;

namespace Zap.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _database;
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWork uow, IMapper mapper)
        {
            _database = uow ?? throw new ArgumentNullException(nameof(uow));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task CreateComment(CommentDTO commentDTO)
        {
            if (commentDTO == null) throw new ArgumentNullException(nameof(commentDTO));
            var comment = _mapper.Map<Comment>(commentDTO);
            if (comment.CreatedAt == default) comment.CreatedAt = DateTime.UtcNow;
            await _database.Comments.AddAsync(comment);
            await _database.SaveAsync();
        }

        public async Task UpdateComment(CommentDTO commentDTO)
        {
            if (commentDTO == null) throw new ArgumentNullException(nameof(commentDTO));
            var comment = await _database.Comments.GetByIdAsync(commentDTO.Id);
            if (comment != null)
            {
                _mapper.Map(commentDTO, comment);
                _database.Comments.Update(comment);
                await _database.SaveAsync();
            }
        }

        public async Task DeleteComment(int id)
        {
            var comment = await _database.Comments.GetByIdAsync(id);
            if (comment != null)
            {
                _database.Comments.Delete(comment);
                await _database.SaveAsync();
            }
        }

        public async Task<CommentDTO?> GetCommentById(int id)
        {
            var comment = await _database.Comments.GetByIdAsync(id);
            if (comment == null)
                return null;
            return _mapper.Map<CommentDTO>(comment);
        }

        public async Task<IEnumerable<CommentDTO>> GetAllComments()
        {
            var comments = await _database.Comments.GetAllAsync();
            return _mapper.Map<IEnumerable<CommentDTO>>(comments);
        }
    }
}