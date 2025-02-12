using System;
using System.Threading.Tasks;
using TodoApi.Infrastructure.Persistence;

namespace TodoApi.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        ITodoRepository Todos { get; }
        ITagRepository Tags { get; }
        Task<int> CompleteAsync();
    }
}
