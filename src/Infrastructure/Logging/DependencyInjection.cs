using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Settings.Configuration;

namespace ArticlesApp.Infrastructure.Logging
{
    public static class DependencyInjection
    {
        public static IHostBuilder AddLogging(this IHostBuilder host)
        {
            //Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine($"SERILOG INTERNAL ERROR: {msg}"));


            return host.UseSerilog((context, services, configuration) => configuration

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
                .ReadFrom.Configuration(
                    context.Configuration,
                    new ConfigurationReaderOptions(Microsoft.Extensions.DependencyModel.DependencyContext.Default))

                // Читаем службы из DI (если нужно для сложных Enrichers)
                .ReadFrom.Services(services)                 
                 
                 // Прописываем логику обогащения в коде, так как оно неизменно для всех сред. А все остальное в appsettings
                 .Enrich.FromLogContext()
                 .Enrich.WithSpan() // Автоматически подхватит Activity (TraceId и SpanId)
                 .Enrich.WithMachineName()
                 .Enrich.WithProperty("Application", "ArticleApp") // Имя сервиса
                 .Enrich.WithProperty("Version", "1.0.0") // Версия сборки
                 
                 .WriteTo.Console(outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [{TraceId}] {Message:lj}{NewLine}{Exception}")

                 // Этот заголовок задает клиент/фронтенд или шлюз.
                 // Используется, чтобы связать цепочку логов между разными сервисами (например, фронтендом и бэкендом или двумя микросервисами).
                 // Позволяет проследить весь путь пользователя через несколько систем/микросервисов.
                 //.Enrich.WithCorrelationIdHeader("X-Correlation-Id") 

                 // Добавляем фильтрацию (пример: не писать логи определенных типов)
                 .Filter.ByExcluding(logEvent => logEvent.Level == LogEventLevel.Information
                    && logEvent.MessageTemplate.Text.Contains("HealthCheck"))
            );
        }
    }
}
