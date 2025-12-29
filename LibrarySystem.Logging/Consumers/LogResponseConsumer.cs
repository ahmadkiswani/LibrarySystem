using LibrarySystem.Common.Messaging;
using LibrarySystem.Logging.DTOs;
using LibrarySystem.Logging.Services;
using MassTransit;

namespace LibrarySystem.Logging.Consumers
{
    public class LogResponseConsumer : IConsumer<LogResponseMessage>
    {
        private readonly MongoLogService _mongo;

        public LogResponseConsumer(MongoLogService mongo)
        {
            _mongo = mongo;
        }

        public async Task Consume(ConsumeContext<LogResponseMessage> context)
        {
            var msg = context.Message;

            await _mongo.SaveResponseAsync(new LogResponseDto
            {
                CorrelationId = msg.CorrelationId,
                Time = msg.Time,
                ServiceName = msg.ServiceName,
                Response = msg.Response
            });
        }
    }
}
