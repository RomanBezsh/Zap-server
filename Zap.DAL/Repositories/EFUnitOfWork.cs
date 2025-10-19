using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zap.DAL.EF;
using Zap.DAL.Entities;
using Zap.DAL.Interfaces;

namespace Zap.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private ZapContext _db;
        private UserRepository _userRepository;
        private UserFollowRepository _userFollowRepository;
        private PostRepository _postRepository;
        private CommentRepository _commentRepository;
        private MediaAttachmentRepository _mediaAttachment;
        
        public EFUnitOfWork(ZapContext context)
        {
            _db = context;
        }

        public IRepository<User> Users
        {
            get {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_db);
                return _userRepository;
            }
        }
        public IRepository<Post> Posts
        {
            get
            {
                if (_postRepository == null)
                    _postRepository = new PostRepository(_db);
                return _postRepository;
            }
        }
        public IRepository<Comment> Comments
        {
            get
            {
                if (_commentRepository == null)
                    _commentRepository = new CommentRepository(_db);
                return _commentRepository;
            }
        }
        public IRepository<MediaAttachment> MediaAttachments
        {
            get
            {
                if (_mediaAttachment == null)
                    _mediaAttachment = new MediaAttachmentRepository(_db);
                return _mediaAttachment;
            }
        }

        public IRepository<UserFollow> UserFollows
        {
            get
            {
                if (_userFollowRepository == null)
                    _userFollowRepository = new UserFollowRepository(_db);
                return _userFollowRepository;
            }
        }
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
