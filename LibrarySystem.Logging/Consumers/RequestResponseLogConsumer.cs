using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using LibrarySystem.Logging.DTOs;
using LibrarySystem.Logging.Services;

namespace LibrarySystem.Logging.Consumers
{
    public class RequestResponseLogConsumer
    {
        private readonly MongoLogService _mongoService;

        public RequestResponseLogConsumer(MongoLogService mongoService)
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
                queue: "request-response-queue",
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (_, ea) =>
            {
                Console.WriteLine(">>> MESSAGE RECEIVED");

                var json = Encoding.UTF8.GetString(ea.Body.ToArray());

                if (json.Contains("\"Request\""))
                {
                    var dto = JsonSerializer.Deserialize<LogRequestDto>(json);
                    await _mongoService.SaveRequestAsync(dto!);
                }
                else
                {
                    var dto = JsonSerializer.Deserialize<LogResponseDto>(json);
                    await _mongoService.SaveResponseAsync(dto!);
                }

                await channel.BasicAckAsync(ea.DeliveryTag, false);
            };

            await channel.BasicConsumeAsync(
                queue: "request-response-queue",
                autoAck: false,
                consumer: consumer
            );
        }
    }
}
