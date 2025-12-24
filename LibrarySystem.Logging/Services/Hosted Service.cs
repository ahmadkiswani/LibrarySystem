using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using LibrarySystem.Logging.Consumers;

namespace LibrarySystem.Logging.Services
{
    public class HostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public HostedService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine(">>> HostedService.ExecuteAsync STARTED");

            using var scope = _scopeFactory.CreateScope();

            var requestConsumer =
                scope.ServiceProvider.GetRequiredService<RequestResponseLogConsumer>();

            var exceptionConsumer =
                scope.ServiceProvider.GetRequiredService<ExceptionLogConsumer>();

            await requestConsumer.StartAsync();
            await exceptionConsumer.StartAsync();

            try
            {
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (TaskCanceledException)
            {
            }
        }
    }
}
