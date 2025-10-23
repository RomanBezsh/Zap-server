using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zap.DAL.Entities;

namespace Zap.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<User> Users { get; }
        IRepository<Post> Posts { get; }
        ICommentRepository Comments { get; }
        IRepository<MediaAttachment> MediaAttachments { get; }
        IRepository<UserFollow> UserFollows { get; }
        IRepository<PostLike> PostLikes { get; }
        IRepository<CommentLike> CommentLikes { get; }
        Task SaveAsync();
    }
}
