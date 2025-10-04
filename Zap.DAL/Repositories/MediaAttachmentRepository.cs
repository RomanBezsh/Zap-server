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
    public class MediaAttachmentRepository : IRepository<MediaAttachment>
    {
        private ZapContext _db;

        public MediaAttachmentRepository(ZapContext context)
        {
            _db = context;
        }

        public async Task<IEnumerable<MediaAttachment>> GetAllAsync()
        {
            return await _db.MediaAttachments.ToListAsync();
        }

        public async Task<MediaAttachment?> GetByIdAsync(int id)
        {
            return await _db.MediaAttachments.FindAsync(id);
        }

        public async Task AddAsync(MediaAttachment entity)
        {
            await _db.MediaAttachments.AddAsync(entity);
        }

        public void Update(MediaAttachment entity)
        {
            _db.MediaAttachments.Update(entity);
        }

        public void Delete(MediaAttachment entity)
        {
            _db.MediaAttachments.Remove(entity);
        }
    }
}
