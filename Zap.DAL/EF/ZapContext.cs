using Microsoft.EntityFrameworkCore;
using Zap.DAL.EF;
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

            // Explicit many-to-many self-reference via join entity UserFollow
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

            // (Keep other mappings/configs as needed)
        }
    }
}