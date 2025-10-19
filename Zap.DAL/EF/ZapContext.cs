using Microsoft.EntityFrameworkCore;
using Zap.DAL.Entities;

namespace Zap.DAL.EF
{
    public class ZapContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<MediaAttachment> MediaAttachments { get; set; }

        public ZapContext(DbContextOptions<ZapContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // MediaAttachment -> Post (optional)
            modelBuilder.Entity<MediaAttachment>()
                .HasOne(m => m.Post)
                .WithMany(p => p.Attachments)
                .HasForeignKey(m => m.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // MediaAttachment -> Comment (optional)
            modelBuilder.Entity<MediaAttachment>()
                .HasOne(m => m.Comment)
                .WithMany(c => c.Attachments)
                .HasForeignKey(m => m.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MediaAttachment>()
                .HasOne(m => m.Post)
                .WithMany(p => p.Attachments)
                .HasForeignKey(m => m.PostId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
