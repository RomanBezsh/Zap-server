using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zap.DAL.Entities;

namespace Zap.DAL.EF
{
    public class ZapContext : DbContext
    {
        public ZapContext(DbContextOptions<ZapContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<MediaAttachment> MediaAttachments { get; set; }
    }
}
