using Microsoft.EntityFrameworkCore;
using Zap.DAL.EF;
using Zap.DAL.Entities;
using Zap.DAL.Interfaces;

namespace Zap.DAL.Repositories
{
    public class UserFollowRepository : IRepository<UserFollow>
    {
        private ZapContext _db;

        public UserFollowRepository(ZapContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<UserFollow>> GetAllAsync()
        {
            return await _db.UserFollows.ToListAsync();
        }

        public async Task<UserFollow?> GetByIdAsync(int id)
        {
            return await _db.UserFollows.FindAsync(id);
        }
        public async Task AddAsync(UserFollow entity)
        {
            await _db.UserFollows.AddAsync(entity);
        }

        public void Update(UserFollow entity)
        {
            _db.UserFollows.Update(entity);
        }

        public void Delete(UserFollow entity)
        {
            _db.UserFollows.Remove(entity);
        }
    }
}