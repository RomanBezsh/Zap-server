using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zap.DAL.EF;
using Zap.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Zap.DAL.Interfaces;

namespace Zap.DAL.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private ZapContext _db;

        public UserRepository(ZapContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _db.Users.FindAsync(id);
        }

        public async Task AddAsync(User entity)
        {
            await _db.Users.AddAsync(entity);
        }

        public void Update(User entity)
        {
            _db.Users.Update(entity);
        }

        public void Delete(User entity)
        {
            _db.Users.Remove(entity);
        }
    }
}
