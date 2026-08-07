using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ArticlesApp.Infrastructure.Common.Extensions
{
    public static class OptionsValidationExtensions
    {
        public static OptionsBuilder<TOptions> AddWithFluentValidation<TOptions, TValidator>(
            this IServiceCollection services,
            string sectionName)
            where TOptions : class
            where TValidator : class, IValidator<TOptions>
        {
            services.AddTransient<IValidator<TOptions>, TValidator>();

            return services.AddOptions<TOptions>()            
                .BindConfiguration(sectionName, options =>
                {
                    // строгая проверка структуры JSON 
                    options.ErrorOnUnknownConfiguration = true; 
                })
                .ValidateWithFluentValidation()
                .ValidateOnStart();
            }

        private static OptionsBuilder<TOptions> ValidateWithFluentValidation<TOptions>(
            this OptionsBuilder<TOptions> optionsBuilder) 
            where TOptions : class
        {            
            optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(
                sp => new FluentValidateOptions<TOptions>(optionsBuilder.Name, sp));

            return optionsBuilder;
        }
    }
}
