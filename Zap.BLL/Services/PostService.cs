using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Zap.BLL.DTO;
using Zap.BLL.Interfaces;
using Zap.DAL.Entities;
using Zap.DAL.Interfaces;

namespace Zap.BLL.Services
{
    public class PostService : IPostService
    {
        IUnitOfWork Database { get; set; }
        private readonly IMapper _mapper;

        public PostService(IUnitOfWork uow, IMapper mapper)
        {
            Database = uow;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task CreatePost(PostDTO postDTO)
        {
            Post post = new Post
            {
                UserId = postDTO.UserId,
                Content = postDTO.Content,
                CreatedAt = postDTO.CreatedAt,
                LikesCount = postDTO.LikesCount,
                RepostsCount = postDTO.RepostsCount,
                ReplyToPostId = postDTO.ReplyToPostId
            };
            await Database.Posts.AddAsync(post);
            await Database.SaveAsync();
        }
        public async Task UpdatePost(PostDTO postDTO)
        {
            var post = await Database.Posts.GetByIdAsync(postDTO.Id);
            if (post != null)
            {
                post.UserId = postDTO.UserId;
                post.Content = postDTO.Content;
                post.CreatedAt = postDTO.CreatedAt;
                post.LikesCount = postDTO.LikesCount;
                post.RepostsCount = postDTO.RepostsCount;
                post.ReplyToPostId = postDTO.ReplyToPostId;
                Database.Posts.Update(post);
                await Database.SaveAsync();
            }
        }
        public async Task DeletePost(int id)
        {
            var post = await Database.Posts.GetByIdAsync(id);
            if (post != null)
            {
                Database.Posts.Delete(post);
            }
        }
        public async Task<PostDTO?> GetPostById(int id)
        {
            var post = await Database.Posts.GetByIdAsync(id);
            if (post == null)
                return null;
            var postDTO = new PostDTO
            {
                Id = post.Id,
                UserId = post.UserId,
                AuthorUsername = post.Author?.Username,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
                LikesCount = post.LikesCount,
                RepostsCount = post.RepostsCount,
                ReplyToPostId = post.ReplyToPostId,
                Comments = post.Comments.Select(c => new CommentDTO
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    AuthorName = c.Author?.Username ?? string.Empty,
                    PostId = c.PostId,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    LikesCount = c.LikesCount
                }).ToList(),
                Attachments = post.Attachments.Select(a => new MediaAttachmentDTO
                {
                    Id = a.Id,
                    PostId = a.PostId,
                    Url = a.Url,
                    MediaType = a.MediaType
                }).ToList()
            };
            return postDTO;
        }
        public async Task<IEnumerable<PostDTO>> GetAllPosts()
        {
            var posts = await Database.Posts.GetAllAsync();
            return _mapper.Map<IEnumerable<PostDTO>>(posts);
        }
    }
}
