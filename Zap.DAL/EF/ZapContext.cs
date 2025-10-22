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
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }

        public ZapContext(DbContextOptions<ZapContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // === Comment -> Author (User) ===
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // === Comment -> Post ===
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // === MediaAttachment -> Post ===
            modelBuilder.Entity<MediaAttachment>()
                .HasOne(m => m.Post)
                .WithMany(p => p.Attachments)
                .HasForeignKey(m => m.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            // === MediaAttachment -> Comment ===
            modelBuilder.Entity<MediaAttachment>()
                .HasOne(m => m.Comment)
                .WithMany(c => c.Attachments)
                .HasForeignKey(m => m.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            // === UserFollow ===
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

            // === POST LIKE CONFIGURATION ===
            modelBuilder.Entity<PostLike>(entity =>
            {
                entity.HasKey(pl => pl.Id);

                entity.HasOne(pl => pl.User)
                    .WithMany(u => u.PostLikes)
                    .HasForeignKey(pl => pl.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(pl => pl.Post)
                    .WithMany(p => p.PostLikes)
                    .HasForeignKey(pl => pl.PostId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(pl => new { pl.UserId, pl.PostId })
                    .IsUnique(); // запретить двойные лайки
            });

            // === COMMENT LIKE CONFIGURATION ===
            modelBuilder.Entity<CommentLike>(entity =>
            {
                entity.HasKey(cl => cl.Id);

                entity.HasOne(cl => cl.User)
                    .WithMany(u => u.CommentLikes)
                    .HasForeignKey(cl => cl.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(cl => cl.Comment)
                    .WithMany(c => c.CommentLikes)
                    .HasForeignKey(cl => cl.CommentId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(cl => new { cl.UserId, cl.CommentId })
                    .IsUnique(); // запретить двойные лайки
            });
        }
    }
}
