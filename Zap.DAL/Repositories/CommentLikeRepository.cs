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
    public class CommentLikeRepository : IRepository<CommentLike>
    {
        private ZapContext _db;

        public CommentLikeRepository(ZapContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<CommentLike>> GetAllAsync()
        {
            return await _db.CommentLikes.ToListAsync();
        }

        public async Task<CommentLike?> GetByIdAsync(int id)
        {
            return await _db.CommentLikes.FindAsync(id);
        }
        public async Task AddAsync(CommentLike entity)
        {
            await _db.CommentLikes.AddAsync(entity);
        }

        public void Update(CommentLike entity)
        {
            _db.CommentLikes.Update(entity);
        }

        public void Delete(CommentLike entity)
        {
            _db.CommentLikes.Remove(entity);
        }
    }
}
