using LibrarySystem.Logging.DTOs;
using LibrarySystem.Logging.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace LibrarySystem.Logging.Services
{

    public class RabbitMqLogService : ILogService
    {
        private readonly IConnection _connection;

        public RabbitMqLogService()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        }

        public async Task LogRequestAsync(LogRequestDto dto)
        {
            await PublishAsync("request-response-queue", dto);
        }

        public async Task LogResponseAsync(LogResponseDto dto)
        {
            await PublishAsync("request-response-queue", dto);
        }

        public async Task LogExceptionAsync(LogExceptionDto dto)
        {
            await PublishAsync("exceptions-queue", dto);
        }

        private async Task PublishAsync(string queueName, object message)
        {
            await using var channel = await _connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: queueName,
                body: body
            );
        }
    }
}