using Application.Abstractions.Caching;
using ArticlesApp.Infrastructure.Cache.Settings;
using ArticlesApp.Infrastructure.Common.Extensions;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ArticlesApp.Infrastructure.Cache
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCache(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            services.AddWithFluentValidation<CacheOptions, CacheOptionsValidator>(CacheOptions.SectionName);                                 

            var cacheSettings = configuration
                .GetSection(CacheOptions.SectionName)
                .Get<CacheOptions>()!;

            // используется в интеграционных тестах 
            //var multiplexer = ConnectionMultiplexer.Connect(cacheSettings.RedisUrl);
            //services.AddSingleton<IConnectionMultiplexer>(multiplexer);

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = cacheSettings.RedisUrl;
                options.InstanceName = cacheSettings.InstanceName;
            });

            services.AddHybridCache(options =>
            {
                // глобальные настройки
                options.DefaultEntryOptions = new HybridCacheEntryOptions
                {                    
                    Expiration = TimeSpan.FromSeconds(cacheSettings.ExpirationSeconds),                         
                    LocalCacheExpiration = TimeSpan.FromSeconds(cacheSettings.LocalCacheExpirationSeconds) 
                };
            });
                       
            services.AddScoped<ICacheService, HybridCacheService>();
            services.AddSingleton<ICacheKeyBuilder, CacheKeyBuilder>();

            return services;
        }
    }
}
