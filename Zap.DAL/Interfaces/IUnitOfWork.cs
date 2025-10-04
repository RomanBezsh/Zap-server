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
        IRepository<Comment> Comments { get; }
        IRepository<MediaAttachment> MediaAttachments { get; }
        Task SaveAsync();
    }
}
