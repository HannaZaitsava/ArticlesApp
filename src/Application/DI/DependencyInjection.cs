using Application.Common.Behaviors;
using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)//, TypeAdapterConfig config)
        {           
            var assembly = typeof(DependencyInjection).Assembly;
            //// добавить конфигурации Mapster слоя Application в общий список конфигураций маппера.
            //// Минус: смешивание конфигов из разных слоев, что может привести Mapster к путанице, если конфиги из разных слоев будут совпадать 
            //config.Scan(assembly);

            // Регистрируем все валидаторы из сборки
            services.AddValidatorsFromAssembly(assembly);
            
            services.AddMediatR(cfg =>
            {
                // Регистрация всех хендлеров из этой сборки
                cfg.RegisterServicesFromAssembly(assembly);
                // Регистрация валидации, логирования и т.д.
                cfg.AddOpenBehavior(typeof(PerformanceBehavior<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));     
                cfg.AddOpenBehavior(typeof(CachingBehavior<,>));
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
