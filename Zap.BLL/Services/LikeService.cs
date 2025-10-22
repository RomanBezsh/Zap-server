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
    public class LikeService : ILikeService
    {
        private readonly IUnitOfWork _database;
        private readonly IMapper _mapper;

        public LikeService(IUnitOfWork uow, IMapper mapper)
        {
            _database = uow ?? throw new ArgumentNullException(nameof(uow));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // === POST LIKES ===
        public async Task TogglePostLikeAsync(int postId, int userId)
        {
            var likes = await _database.PostLikes.GetAllAsync();
            var existing = likes.FirstOrDefault(l => l.PostId == postId && l.UserId == userId);

            if (existing != null)
                _database.PostLikes.Delete(existing);
            else
                await _database.PostLikes.AddAsync(new PostLike { PostId = postId, UserId = userId });

            await _database.SaveAsync();
        }

        public async Task<int> GetPostLikesCountAsync(int postId)
        {
            var likes = await _database.PostLikes.GetAllAsync();
            return likes.Count(l => l.PostId == postId);
        }

        public async Task<bool> IsPostLikedByUserAsync(int postId, int userId)
        {
            var likes = await _database.PostLikes.GetAllAsync();
            return likes.Any(l => l.PostId == postId && l.UserId == userId);
        }

        public async Task<IEnumerable<PostLikeDTO>> GetAllPostLikesAsync()
        {
            var likes = await _database.PostLikes.GetAllAsync();
            return _mapper.Map<IEnumerable<PostLikeDTO>>(likes);
        }

        // === COMMENT LIKES ===
        public async Task ToggleCommentLikeAsync(int commentId, int userId)
        {
            var likes = await _database.CommentLikes.GetAllAsync();
            var existing = likes.FirstOrDefault(l => l.CommentId == commentId && l.UserId == userId);

            if (existing != null)
                _database.CommentLikes.Delete(existing);
            else
                await _database.CommentLikes.AddAsync(new CommentLike { CommentId = commentId, UserId = userId });

            await _database.SaveAsync();
        }

        public async Task<int> GetCommentLikesCountAsync(int commentId)
        {
            var likes = await _database.CommentLikes.GetAllAsync();
            return likes.Count(l => l.CommentId == commentId);
        }

        public async Task<bool> IsCommentLikedByUserAsync(int commentId, int userId)
        {
            var likes = await _database.CommentLikes.GetAllAsync();
            return likes.Any(l => l.CommentId == commentId && l.UserId == userId);
        }

        public async Task<IEnumerable<CommentLikeDTO>> GetAllCommentLikesAsync()
        {
            var likes = await _database.CommentLikes.GetAllAsync();
            return _mapper.Map<IEnumerable<CommentLikeDTO>>(likes);
        }
    }

}
