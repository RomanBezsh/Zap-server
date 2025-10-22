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
    public class PostLikeRepository : IRepository<PostLike>
    {
        private ZapContext _db;

        public PostLikeRepository(ZapContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<PostLike>> GetAllAsync()
        {
            return await _db.PostLikes.ToListAsync();
        }

        public async Task<PostLike?> GetByIdAsync(int id)
        {
            return await _db.PostLikes.FindAsync(id);
        }
        public async Task AddAsync(PostLike entity)
        {
            await _db.PostLikes.AddAsync(entity);
        }

        public void Update(PostLike entity)
        {
            _db.PostLikes.Update(entity);
        }

        public void Delete(PostLike entity)
        {
            _db.PostLikes.Remove(entity);
        }
    }
}
