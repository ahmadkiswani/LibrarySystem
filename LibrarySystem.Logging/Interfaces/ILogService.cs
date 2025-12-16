using LibrarySystem.Logging.DTOs;

namespace LibrarySystem.Logging.Interfaces
{
    public interface ILogService
    {
        Task LogRequestAsync(LogRequestDto dto);
        Task LogResponseAsync(LogResponseDto dto);
        Task LogExceptionAsync(LogExceptionDto dto);
    }
}
