using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Domain.Entities;

namespace TodoApi.Infrastructure.Persistence
{
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _context;

        public TodoRepository(TodoContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TodoItem>> GetAllAsync()
        {
            return await _context.Todos
                .Include(t => t.TodoTags)
                .ThenInclude(tt => tt.Tag)
                .ToListAsync();
        }

        public async Task<TodoItem> GetByIdAsync(int id)
        {
            return await _context.Todos
                .Include(t => t.TodoTags)
                .ThenInclude(tt => tt.Tag)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(TodoItem todo)
        {
            await _context.Todos.AddAsync(todo);
        }

        public void Remove(TodoItem todo)
        {
            _context.Todos.Remove(todo);
        }
    }
}
