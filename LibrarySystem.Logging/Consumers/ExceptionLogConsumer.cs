using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using LibrarySystem.Logging.DTOs;
using LibrarySystem.Logging.Services;

namespace LibrarySystem.Logging.Consumers
{
    public class ExceptionLogConsumer
    {
        private readonly MongoLogService _mongoService;

        public ExceptionLogConsumer(MongoLogService mongoService)
        {
            _mongoService = mongoService;
        }

        public async Task StartAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: "exceptions-queue",
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (_, ea) =>
            {
                Console.WriteLine(">>> EXCEPTION MESSAGE RECEIVED");

                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var dto = JsonSerializer.Deserialize<LogExceptionDto>(json);

                await _mongoService.SaveExceptionAsync(dto!);

                await channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            await channel.BasicConsumeAsync(
                queue: "exceptions-queue",
                autoAck: false,
                consumer: consumer
            );
        }
    }
}
