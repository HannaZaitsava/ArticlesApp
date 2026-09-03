namespace ArticlesApp.Infrastructure.DataAccess.Abstractions
{
    public interface IDbInitializer
    {
        Task MigrateAsync(CancellationToken cancellationToken = default);
    }
}
