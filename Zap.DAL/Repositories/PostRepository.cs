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
    public class PostRepository : IRepository<Post>
    {
        private ZapContext _db;

        public PostRepository(ZapContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _db.Posts.Include(p => p.UserId).ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            return await _db.Posts.FindAsync(id);
        }

        public async Task AddAsync(Post entity)
        {
            await _db.Posts.AddAsync(entity);
        }

        public void Update(Post entity)
        {
            _db.Posts.Update(entity);
        }

        public void Delete(Post entity)
        {
            _db.Posts.Remove(entity);
        }

    }
}
