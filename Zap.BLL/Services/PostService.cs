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
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public PostService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow ?? throw new ArgumentNullException(nameof(uow));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task CreatePost(PostDTO postDTO)
        {
            if (postDTO == null) throw new ArgumentNullException(nameof(postDTO));
            var post = _mapper.Map<Post>(postDTO);
            if (post.CreatedAt == default) post.CreatedAt = DateTime.UtcNow;
            await _db.Posts.AddAsync(post);
            await _db.SaveAsync();
        }

        public async Task UpdatePost(PostDTO postDTO)
        {
            if (postDTO == null) throw new ArgumentNullException(nameof(postDTO));
            var post = await _db.Posts.GetByIdAsync(postDTO.Id);
            if (post != null)
            {
                _mapper.Map(postDTO, post);
                _db.Posts.Update(post);
                await _db.SaveAsync();
            }
        }

        public async Task DeletePost(int id)
        {
            var post = await _db.Posts.GetByIdAsync(id);
            if (post != null)
            {
                _db.Posts.Delete(post);
                await _db.SaveAsync();
            }
        }

        public async Task<PostDTO?> GetPostById(int id)
        {
            var post = await _db.Posts.GetByIdAsync(id);
            if (post == null)
                return null;
            return _mapper.Map<PostDTO>(post);
        }

        public async Task<IEnumerable<PostDTO>> GetAllPosts()
        {
            var posts = await _db.Posts.GetAllAsync();
            return _mapper.Map<IEnumerable<PostDTO>>(posts);
        }
    }
}