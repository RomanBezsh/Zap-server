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
            // Include navigation properties (Author is the navigation, not UserId)
            return await _db.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments) // optional: include comments
                    .ThenInclude(c => c.Attachments) // optional: include comment attachments
                .Include(p => p.Attachments) // optional: include post attachments
                .ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            // Use Include when you need navigation properties for a single entity
            return await _db.Posts
                .Include(p => p.Author) 
                .Include(p => p.Comments)
                    .ThenInclude(c => c.Attachments)
                .Include(p => p.Attachments)
                .FirstOrDefaultAsync(p => p.Id == id);
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
