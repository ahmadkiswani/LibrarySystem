using LibrarySystem.UserIdentity.DTOs;

namespace LibrarySystem.UserIdentity.Services.Interface
{
    public interface IUserService
    {
        Task RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}
