using ArticlesApp.Application.Common.Behaviors;
using ArticlesApp.Application.Common.Caching;
using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace ArticlesApp.Application.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {           
            var assembly = typeof(DependencyInjection).Assembly;
            //// добавить конфигурации Mapster слоя Application в общий список конфигураций маппера.
            //// Минус: смешивание конфигов из разных слоев, что может привести Mapster к путанице, если конфиги из разных слоев будут совпадать 
            //config.Scan(assembly);

            // Регистрируем все валидаторы из сборки
            services.AddValidatorsFromAssembly(assembly);

            services.AddScoped<ICacheInvalidationContext, CacheInvalidationContext>();

            services.AddMediatR(cfg =>
            {
                // Регистрация всех хендлеров из этой сборки
                cfg.RegisterServicesFromAssembly(assembly);
                // Регистрация валидации, логирования и т.д.
                cfg.AddOpenBehavior(typeof(PerformanceBehavior<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));     
                cfg.AddOpenBehavior(typeof(CachingBehavior<,>));
                cfg.AddOpenBehavior(typeof(CacheInvalidationBehavior<,>));
            });            

            return services;
        }

        public static void  AddApplicationMapperConfigurations(this TypeAdapterConfig config)
        {
            var assembly = typeof(DependencyInjection).Assembly;            
            // добавить конфигурации Mapster слоя Application в общий список конфигураций маппера.
            // Минус: смешивание конфигов из разных слоев, что может привести Mapster к путанице, если конфиги из разных слоев будут совпадать 
            config.Scan(assembly);
        }
    }
}
