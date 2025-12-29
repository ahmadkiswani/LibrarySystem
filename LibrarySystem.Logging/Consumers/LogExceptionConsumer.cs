using LibrarySystem.Common.Messaging;
using LibrarySystem.Logging.DTOs;
using LibrarySystem.Logging.Services;
using MassTransit;

namespace LibrarySystem.Logging.Consumers
{
    public class LogExceptionConsumer : IConsumer<LogExceptionMessage>
    {
        private readonly MongoLogService _mongo;

        public LogExceptionConsumer(MongoLogService mongo)
        {
            _mongo = mongo;
        }

        public async Task Consume(ConsumeContext<LogExceptionMessage> context)
        {
            var msg = context.Message;

            await _mongo.SaveExceptionAsync(new LogExceptionDto
            {
                CorrelationId = msg.CorrelationId,
                Time = msg.Time,
                ServiceName = msg.ServiceName,
                Message = msg.Message,
                StackTrace = msg.StackTrace,
                Request = msg.Request,
                Response = msg.Response,
                ErrorType = msg.ErrorType
            });
        }
    }
}
