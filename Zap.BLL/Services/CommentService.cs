using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zap.BLL.DTO;
using Zap.BLL.Interfaces;
using Zap.DAL.Entities;
using Zap.DAL.Interfaces;
using Zap.DAL.Repositories;

namespace Zap.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow ?? throw new ArgumentNullException(nameof(uow));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task CreateComment(CommentDTO commentDTO)
        {
            if (commentDTO == null) throw new ArgumentNullException(nameof(commentDTO));

            var comment = _mapper.Map<Comment>(commentDTO);
            if (comment.CreatedAt == default)
                comment.CreatedAt = DateTime.UtcNow;

            if (commentDTO.ParentCommentId.HasValue)
            {
                var parent = await _db.Comments.GetByIdAsync(commentDTO.ParentCommentId.Value);
                if (parent == null)
                    throw new Exception("Parent comment not found.");

                comment.ParentCommentId = parent.Id;
            }

            await _db.Comments.AddAsync(comment);
            await _db.SaveAsync();
        }


        public async Task UpdateComment(CommentDTO commentDTO)
        {
            var comment = await _db.Comments.GetByIdAsync(commentDTO.Id);
            if (comment == null) return;

            comment.Content = commentDTO.Content;
            await _db.SaveAsync();
        }

        public async Task DeleteComment(int id)
        {
            var comment = await _db.Comments.GetByIdAsync(id);
            if (comment != null)
            {
                _db.Comments.Delete(comment);
                await _db.SaveAsync();
            }
        }

        public async Task<CommentDTO?> GetCommentById(int id)
        {
            var comment = await _db.Comments.GetByIdAsync(id);
            if (comment == null)
                return null;
            return _mapper.Map<CommentDTO>(comment);
        }

        public async Task<IEnumerable<CommentDTO>> GetAllComments()
        {
            var comments = await _db.Comments.GetAllAsync();
            return _mapper.Map<IEnumerable<CommentDTO>>(comments);
        }

        public async Task<IEnumerable<CommentDTO>> GetAllCommentsForPost(int postId)
        {
            var post = await _db.Posts.GetByIdAsync(postId);
            if (post == null)
                return Enumerable.Empty<CommentDTO>();

            var commentRepo = _db.Comments as CommentRepository;
            if (commentRepo == null)
                throw new Exception("Cannot access CommentRepository");

            var comments = await commentRepo.GetCommentsForPostAsync(postId);
            return _mapper.Map<IEnumerable<CommentDTO>>(comments);
        }

    }
}