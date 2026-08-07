using ArticlesAPI.Extensions.DI;
using ArticlesApp.Infrastructure.Cache.Settings;
using ArticlesApp.Infrastructure.DataAccess.Settings;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ArticlesAPI.Extensions.DI
{
    public static class HealthChecksExtensions
    {
        public static WebApplicationBuilder AddHealthChecks(this WebApplicationBuilder builder)
        {
            var postgreSQLConneсtionString = builder.Configuration
                .GetSection(DatabaseOptions.SectionName)
                .Get<DatabaseOptions>()!.BaseDbConnection;

            var redisConnectionString = builder.Configuration
                .GetSection(CacheOptions.SectionName)
                .Get<CacheOptions>()!.RedisUrl;

            builder.Services.AddHealthChecks()
                .AddNpgSql(
                    connectionString: postgreSQLConneсtionString!,
                    name: "PostgreSQL Database",
                    tags: ["db"])

                .AddRedis(
                    redisConnectionString: redisConnectionString!,
                    name: "Redis Hybrid Cache",
                    failureStatus: HealthStatus.Degraded, // Redis — некритическая зависимость (есть fallback в БД)
                    tags: ["cache"]);  

            builder.Services.AddHealthChecksUI(setup =>
            {
                setup.AddHealthCheckEndpoint("API Health", "/health");

                setup.SetEvaluationTimeInSeconds(20); // Опрос раз в 20 секунд
                setup.MaximumHistoryEntriesPerEndpoint(100); // Хранить в БД только последние 100 проверок, чтобы БД не рослa
                setup.SetMinimumSecondsBetweenFailureNotifications(50); // Регулирует частоту уведомлений для вебхуков   

                var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
                if (isDocker)
                {
                    setup.UseApiEndpointHttpMessageHandler(sp =>
                    {
                        return new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback =
                                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                        };
                    });
                }
            })             
            .AddPostgreSqlStorage(postgreSQLConneсtionString!, options =>
            {
                // Для некоторых версий провайдера настройка делается так:
                // options.MigrationsHistoryTable("__HealthCheckMigrationsHistory", "health");
                // Отключаем проверку ожидающих изменений в модели для этого контекста
                options.ConfigureWarnings(x => x.Ignore(RelationalEventId.PendingModelChangesWarning));
            });
           
            return builder;
        }
    }
}
