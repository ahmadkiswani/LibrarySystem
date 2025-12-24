using LibrarySystem.Logging.DTOs;

namespace LibrarySystem.Logging.Interfaces
{
    public interface ILogEventPublisher
    {
        Task PublishRequestAsync(LogRequestDto dto);
        Task PublishResponseAsync(LogResponseDto dto);
        Task PublishExceptionAsync(LogExceptionDto dto);
    }
}
