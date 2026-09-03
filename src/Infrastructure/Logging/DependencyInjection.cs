using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Settings.Configuration;

namespace ArticlesApp.Infrastructure.Logging
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCustomLogging(this IServiceCollection services)
        {
            /*
            Метод .ReadFrom.Configuration(...) отвечает за чтение текста из JSON-файла. 
            То, что написано внутри его скобок, отвечает за поиск кода для этого текста:
               1) .ReadFrom.Configuration(context.Configuration) — читать настройки из JSON, а список пакетов указывается вручную текстом в блоке "Using" в appsettings.
                   "Serilog": {
                     "Using": [
                       "Serilog.Sinks.Console",
                       "Serilog.Sinks.File",
                       "Serilog.Sinks.Seq"
                     ],
               2) .ReadFrom.Configuration(context.Configuration, new ConfigurationReaderOptions { ... }) — читать настройки из JSON, 
                   а Nuget-пакеты найдет в проекте автоматически (блок "Using" больше не нужен).
            */

            //Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine($"SERILOG INTERNAL ERROR: {msg}"));

            services.AddSerilog((serviceProvider, loggerConfiguration) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                // Используем GetService, так как в некоторых тестах хоста может не быть
                var environment = serviceProvider.GetService<IHostEnvironment>();

                var environmentName = environment?.EnvironmentName ?? "Production";
                var applicationName = environment?.ApplicationName ?? "UnknownService";
                var version = GetApplicationVersion();

                loggerConfiguration
                    // Автоматически находит NuGet-пакеты синков в проекте (блок "Using" в JSON больше не нужен)
                    .ReadFrom.Configuration(
                        configuration,
                        new ConfigurationReaderOptions(DependencyContext.Default))

                    // Читаем службы из DI для сложных Enrichers
                    .ReadFrom.Services(serviceProvider)

                    .Enrich.FromLogContext()
                    .Enrich.WithSpan()         // Автоматически подхватит Activity (TraceId и SpanId)
                    .Enrich.WithMachineName()
                    .Enrich.WithProperty("Application", applicationName)
                    .Enrich.WithProperty("Environment", environmentName)
                    .Enrich.WithProperty("Version", version)
                    .Enrich.FromLogContext()
                    .WriteTo.Console(new CompactJsonFormatter())
                    // Пример фильтрации: исключаем спам от HealthCheck на уровне Information
                    .Filter.ByExcluding(logEvent =>
                        logEvent.Level == LogEventLevel.Information &&
                        logEvent.MessageTemplate.Text.Contains("HealthCheck"));
            });

            return services;
        }

        private static string GetApplicationVersion()
        {
            var version = Environment.GetEnvironmentVariable("APP_VERSION");

            if (string.IsNullOrEmpty(version))
            {
                version = Assembly.GetEntryAssembly()?
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                    .InformationalVersion
                    ?? Assembly.GetEntryAssembly()?.GetName().Version?.ToString()
                    ?? "1.0.0";
            }

            if (version.Contains('+'))
            {
                version = version.Split('+')[0];
            }

            return version;
        }
    }
}
