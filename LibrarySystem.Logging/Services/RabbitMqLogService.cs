using LibrarySystem.Common.LibraryQueues;
using LibrarySystem.Logging.DTOs;
using LibrarySystem.Logging.Interfaces;
using MassTransit;

public class RabbitMqLogEventPublisher : ILogEventPublisher
{
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public RabbitMqLogEventPublisher(ISendEndpointProvider sendEndpointProvider)
    {
        _sendEndpointProvider = sendEndpointProvider;
    }

    public async Task PublishRequestAsync(LogRequestDto dto)
    {
        var endpoint = await _sendEndpointProvider
            .GetSendEndpoint(new Uri($"queue:{LoggingQueues.RequestResponse}"));

        await endpoint.Send(dto);
    }

    public async Task PublishResponseAsync(LogResponseDto dto)
    {
        var endpoint = await _sendEndpointProvider
            .GetSendEndpoint(new Uri($"queue:{LoggingQueues.RequestResponse}"));

        await endpoint.Send(dto);
    }

    public async Task PublishExceptionAsync(LogExceptionDto dto)
    {
        var endpoint = await _sendEndpointProvider
            .GetSendEndpoint(new Uri($"queue:{LoggingQueues.Exceptions}"));

        await endpoint.Send(dto);
    }
}
