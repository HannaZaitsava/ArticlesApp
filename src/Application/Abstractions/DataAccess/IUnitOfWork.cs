namespace Application.Abstractions.DataAccess
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {        
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        Task BeginTransactionAsync(CancellationToken ct = default);
        Task CommitTransactionAsync(CancellationToken ct = default);
        Task RollbackTransactionAsync(CancellationToken ct = default);
    }
}
