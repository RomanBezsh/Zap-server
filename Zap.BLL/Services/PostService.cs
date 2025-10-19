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
        private readonly IUnitOfWork _database;
        private readonly IMapper _mapper;

        public PostService(IUnitOfWork uow, IMapper mapper)
        {
            _database = uow ?? throw new ArgumentNullException(nameof(uow));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task CreatePost(PostDTO postDTO)
        {
            if (postDTO == null) throw new ArgumentNullException(nameof(postDTO));
            var post = _mapper.Map<Post>(postDTO);
            if (post.CreatedAt == default) post.CreatedAt = DateTime.UtcNow;
            await _database.Posts.AddAsync(post);
            await _database.SaveAsync();
        }

        public async Task UpdatePost(PostDTO postDTO)
        {
            if (postDTO == null) throw new ArgumentNullException(nameof(postDTO));
            var post = await _database.Posts.GetByIdAsync(postDTO.Id);
            if (post != null)
            {
                _mapper.Map(postDTO, post);
                _database.Posts.Update(post);
                await _database.SaveAsync();
            }
        }

        public async Task DeletePost(int id)
        {
            var post = await _database.Posts.GetByIdAsync(id);
            if (post != null)
            {
                _database.Posts.Delete(post);
                await _database.SaveAsync();
            }
        }

        public async Task<PostDTO?> GetPostById(int id)
        {
            var post = await _database.Posts.GetByIdAsync(id);
            if (post == null)
                return null;
            return _mapper.Map<PostDTO>(post);
        }

        public async Task<IEnumerable<PostDTO>> GetAllPosts()
        {
            var posts = await _database.Posts.GetAllAsync();
            return _mapper.Map<IEnumerable<PostDTO>>(posts);
        }
    }
}