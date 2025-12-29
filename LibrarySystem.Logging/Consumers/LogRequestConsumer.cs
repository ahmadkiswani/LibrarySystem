using MassTransit;
using LibrarySystem.Common.Messaging;
using LibrarySystem.Logging.DTOs;
using LibrarySystem.Logging.Services;

namespace LibrarySystem.Logging.Consumers
{
    public class LogRequestConsumer : IConsumer<LogRequestMessage>
    {
        private readonly MongoLogService _mongo;

        public LogRequestConsumer(MongoLogService mongo)
        {
            _mongo = mongo;
        }

        public async Task Consume(ConsumeContext<LogRequestMessage> context)
        {
            var msg = context.Message;

            await _mongo.SaveRequestAsync(new LogRequestDto
            {
                CorrelationId = msg.CorrelationId,
                Time = msg.Time,
                ServiceName = msg.ServiceName,
                Request = msg.Request
            });
        }
    }
}
