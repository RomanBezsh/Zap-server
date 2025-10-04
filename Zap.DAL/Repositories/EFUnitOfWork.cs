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
        private UserRepository? _userRepository;
        private PostRepository? _postRepository;
        private CommentRepository? _commentRepository;
        private MediaAttachmentRepository? _mediaAttachment;
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
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
