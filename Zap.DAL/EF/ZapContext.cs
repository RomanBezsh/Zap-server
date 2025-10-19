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
        public DbSet<UserFollow> UserFollows { get; set; }

        public ZapContext(DbContextOptions<ZapContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Comment -> Author (User) - no cascade
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Comment -> Post: explicit (choose Cascade if you want deleting a Post to delete its Comments)
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // MediaAttachment -> Post (optional)
            // Use NO ACTION to ensure DB will not try to cascade deletes via Post -> MediaAttachment
            modelBuilder.Entity<MediaAttachment>()
                .HasOne(m => m.Post)
                .WithMany(p => p.Attachments)
                .HasForeignKey(m => m.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            // MediaAttachment -> Comment (optional)
            // Keep cascade here if you want attachments removed when comment is removed
            modelBuilder.Entity<MediaAttachment>()
                .HasOne(m => m.Comment)
                .WithMany(c => c.Attachments)
                .HasForeignKey(m => m.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            // UserFollow join config (unchanged)
            modelBuilder.Entity<UserFollow>()
                .HasKey(uf => new { uf.FollowerId, uf.FollowedId });

            modelBuilder.Entity<UserFollow>()
                .HasOne(uf => uf.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(uf => uf.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserFollow>()
                .HasOne(uf => uf.Followed)
                .WithMany(u => u.Followers)
                .HasForeignKey(uf => uf.FollowedId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}