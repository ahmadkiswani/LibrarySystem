using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using LibrarySystem.Logging.Consumers;

namespace LibrarySystem.Logging.Services
{
    public class LoggingHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public LoggingHostedService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();

            var requestConsumer =
                scope.ServiceProvider.GetRequiredService<RequestResponseLogConsumer>();

            var exceptionConsumer =
                scope.ServiceProvider.GetRequiredService<ExceptionLogConsumer>();

            await requestConsumer.StartAsync();
            await exceptionConsumer.StartAsync();

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
//hangfierjobs