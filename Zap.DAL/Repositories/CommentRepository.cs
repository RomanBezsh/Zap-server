using Microsoft.EntityFrameworkCore;
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
    public class CommentRepository : ICommentRepository
    {
        private ZapContext _db;

        public CommentRepository(ZapContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _db.Comments.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _db.Comments.FindAsync(id);
        }

        public async Task AddAsync(Comment entity)
        {
            await _db.Comments.AddAsync(entity);
        }

        public void Update(Comment entity)
        {
            _db.Comments.Update(entity);
        }

        public void Delete(Comment entity)
        {
            _db.Comments.Remove(entity);
        }

        public async Task<List<Comment>> GetCommentsForPostAsync(int postId)
        {
            return await _db.Comments
                .Where(c => c.PostId == postId && c.ParentCommentId == null)
                .Include(c => c.Author)
                .Include(c => c.Attachments)
                .Include(c => c.CommentLikes)
                .Include(c => c.Replies)
                    .ThenInclude(r => r.Author)
                .Include(c => c.Replies)
                    .ThenInclude(r => r.CommentLikes)
                .ToListAsync();
        }

    }
}
