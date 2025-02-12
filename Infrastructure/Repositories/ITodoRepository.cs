using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Domain.Entities;

namespace TodoApi.Infrastructure.Persistence
{
    public interface ITodoRepository
    {
        Task<IEnumerable<TodoItem>> GetAllAsync();
        Task<TodoItem> GetByIdAsync(int id);
        Task AddAsync(TodoItem todo);
        void Remove(TodoItem todo);
    }
}
