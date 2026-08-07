using Application.Abstractions.DataAccess;
using ArticlesApp.Infrastructure.Common.Extensions;
using ArticlesApp.Infrastructure.DataAccess.Abstractions;
using ArticlesApp.Infrastructure.DataAccess.DbContext;
using ArticlesApp.Infrastructure.DataAccess.DbContext.Interceptors;
using ArticlesApp.Infrastructure.DataAccess.Repositories;
using ArticlesApp.Infrastructure.DataAccess.Repositories.ConcreteRepositories;
using ArticlesApp.Infrastructure.DataAccess.Settings;
using ArticlesApp.Infrastructure.DataAccess.UOW;
using Domain.Entities;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ArticlesApp.Infrastructure.DataAccess.DI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataAccessServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
        {
            AddDatabase(services, configuration, environment);

            AddIdentity(services);

            AddRepositories(services);

            services.AddScoped<IDbInitializer, DbInitializer>();

            return services;
        }

        private static void AddDatabase(
            IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment environment)
        {
            services.AddScoped<AuditAndSoftDeleteInterceptor>();

            services.AddWithFluentValidation<DatabaseOptions, DatabaseOptionsValidator>(DatabaseOptions.SectionName);

            var dbConnetionString = configuration
                .GetSection(DatabaseOptions.SectionName)
                .Get<DatabaseOptions>()!.BaseDbConnection;
           
            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                options.UseNpgsql(dbConnetionString);
               
                options.AddInterceptors(
                    sp.GetRequiredService<AuditAndSoftDeleteInterceptor>());

                if (environment.IsProduction()) 
                    options.EnableSensitiveDataLogging();

            }, ServiceLifetime.Scoped);                       

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private static void AddIdentity(IServiceCollection services)
        {
            // Настройка хранения ключей шифрования в БД
            services.AddDataProtection().PersistKeysToDbContext<AppDbContext>();

            services.AddIdentityCore<User>(opt =>
            {
                opt.Password.RequiredLength = 7;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.User.RequireUniqueEmail = true;
                opt.SignIn.RequireConfirmedEmail = true;
            })
               .AddRoles<IdentityRole<Guid>>()
               .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
               .AddRoleValidator<RoleValidator<IdentityRole<Guid>>>()
               .AddEntityFrameworkStores<AppDbContext>()
               .AddDefaultTokenProviders(); // this method requires the IDataProtectionProvider

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                //opt.TokenLifespan = TimeSpan.FromDays(365 * 100)); // условно бесконечный токен (на 100 лет) - для тестирования
                opt.TokenLifespan = TimeSpan.FromHours(2));
        }

        private static void AddRepositories(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>))
                .AddScoped<IArticleRepository, ArticleRepository>()
                .AddScoped<IArticleCategoryRepository, ArticleCategoryRepository>()
                .AddScoped<ICommentRepository, CommentRepository>()
                .AddScoped<ITagRepository, TagRepository>();
                //.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
