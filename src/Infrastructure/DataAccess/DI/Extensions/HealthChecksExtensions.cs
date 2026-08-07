using ArticlesApp.Infrastructure.DataAccess.DbContext;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArticlesApp.Infrastructure.DataAccess.DI.Extensions
{
    public static class HealthChecksExtensions
    {
        public static IServiceCollection AddHealthChecksExtensions(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddHealthChecks()
                .AddNpgSql(connectionString!, name: "postgres", tags: new[] { "db" })
                .AddDbContextCheck<AppDbContext>(name: "ef_context", tags: new[] { "db" });
                // Здесь же регистрируем наш кастомный чек сидинга
                //.AddCheck<DatabaseSeedingHealthCheck>("seeding_status", tags: new[] { "readiness" });                           

            return services;
        }
    }
}
