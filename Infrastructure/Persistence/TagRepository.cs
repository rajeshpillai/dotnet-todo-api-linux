using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Domain.Entities;

namespace TodoApi.Infrastructure.Persistence
{
    public class TagRepository : ITagRepository
    {
        private readonly TodoContext _context;

        public TagRepository(TodoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag> GetByIdAsync(int id)
        {
            return await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(Tag tag)
        {
            await _context.Tags.AddAsync(tag);
        }

        public void Remove(Tag tag)
        {
            _context.Tags.Remove(tag);
        }
    }
}
