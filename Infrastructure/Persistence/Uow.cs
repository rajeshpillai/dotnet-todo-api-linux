using System.Threading.Tasks;
using TodoApi.Infrastructure.Persistence;

namespace TodoApi.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TodoContext _context;
        public ITodoRepository Todos { get; }
        public ITagRepository Tags { get; }

        public UnitOfWork(TodoContext context, ITodoRepository todoRepository, ITagRepository tagRepository)
        {
            _context = context;
            Todos = todoRepository;
            Tags = tagRepository;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
