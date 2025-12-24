using LibrarySystem.UserIdentity.DTOs;

namespace LibrarySystem.UserIdentity.Iinterface
{
    public interface IUserService
    {
        Task RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
    }
}
