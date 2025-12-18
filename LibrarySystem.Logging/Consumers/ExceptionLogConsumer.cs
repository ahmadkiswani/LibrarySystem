using LibrarySystem.Logging.DTOs;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Text;
using LibrarySystem.Logging.Services;



public class ExceptionLogConsumer
{
    private readonly MongoLogService _mongoService;

    public ExceptionLogConsumer(MongoLogService mongoService)
    {
        _mongoService = mongoService;
    }

    public async Task StartAsync()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
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
            var json = Encoding.UTF8.GetString(ea.Body.ToArray());
            var dto = JsonSerializer.Deserialize<LogExceptionDto>(json);

            await _mongoService.LogExceptionAsync(dto!);
            await channel.BasicAckAsync(ea.DeliveryTag, false);
        };

        await channel.BasicConsumeAsync(
            queue: "exceptions-queue",
            autoAck: false,
            consumer: consumer
        );
    }
}
