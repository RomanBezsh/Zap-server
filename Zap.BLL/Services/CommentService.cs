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

namespace Zap.BLL.Services
{
    public class CommentService : ICommentService
    {
        IUnitOfWork Database { get; set; }
        public CommentService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public async Task CreateComment(CommentDTO commentDTO)
        {
            Comment comment = new Comment
            {
                PostId = commentDTO.PostId,
                UserId = commentDTO.UserId,
                Content = commentDTO.Content,
                CreatedAt = commentDTO.CreatedAt
            };
            await Database.Comments.AddAsync(comment);
            await Database.SaveAsync();
        }
        public async Task UpdateComment(CommentDTO commentDTO)
        {
            var comment = await Database.Comments.GetByIdAsync(commentDTO.Id);
            if (comment != null)
            {
                comment.PostId = commentDTO.PostId;
                comment.UserId = commentDTO.UserId;
                comment.Content = commentDTO.Content;
                comment.CreatedAt = commentDTO.CreatedAt;
                Database.Comments.Update(comment);
            }
            await Database.SaveAsync();
        }
        public async Task DeleteComment(int id)
        {
            var comment = await Database.Comments.GetByIdAsync(id);
            if (comment != null)
            {
                Database.Comments.Delete(comment);
            }
            await Database.SaveAsync();
        }
        public async Task<CommentDTO?> GetCommentById(int id)
        {
            var comment = await Database.Comments.GetByIdAsync(id);
            if (comment != null)
            {
                return new CommentDTO
                {
                    Id = comment.Id,
                    PostId = comment.PostId,
                    UserId = comment.UserId,
                    Content = comment.Content,
                    CreatedAt = comment.CreatedAt
                };
            }
            return null;
        }
        public async Task<IEnumerable<CommentDTO>> GetAllComments()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Comment, CommentDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDTO>>(await Database.Comments.GetAllAsync());
        }
    }
}
