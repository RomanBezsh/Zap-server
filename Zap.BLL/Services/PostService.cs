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
            Console.WriteLine($"🟡 CreatePost вызван в сервисе. Content: {postDTO.Content}, UserId: {postDTO.UserId}");

            var post = _mapper.Map<Post>(postDTO);
            Console.WriteLine($"🧩 После маппинга: Content={post.Content}, UserId={post.UserId}");

            await _db.Posts.AddAsync(post);
            Console.WriteLine("🧠 AddAsync прошёл");

            await _db.SaveAsync();
            Console.WriteLine("💾 SaveAsync вызван");
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

        public async Task<IEnumerable<PostDTO>> GetPostsByUserAsync(int userId)
        {
            var posts = await _db.Posts.GetAllAsync();
            var userPosts = posts
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt);

            return _mapper.Map<IEnumerable<PostDTO>>(userPosts);
        }

    }
}