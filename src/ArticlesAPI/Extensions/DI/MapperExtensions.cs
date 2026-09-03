using System.Reflection;
using ArticlesApp.Application.DI;
using Mapster;

namespace ArticlesApp.ArticlesAPI.Extensions.DI
{
    public static class MapperExtensions
    {
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            var config = new TypeAdapterConfig();
            config.Scan(Assembly.GetExecutingAssembly());            
            services.AddSingleton(config);
            services.AddMapster();// AddMapster по умолчанию работает с ServiceMapper => .AddScoped<IMapper, ServiceMapper>()

            // добавляем конфигурации из слоя Application
            config.AddApplicationMapperConfigurations();

            return services;
        }
    }
}
