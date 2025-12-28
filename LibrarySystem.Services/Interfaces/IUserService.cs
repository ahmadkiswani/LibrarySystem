using LibrarySystem.Shared.DTOs.UserDtos;
using LibrarySystem.Shared.DTOs.UserDtos.LibrarySystem.Shared.DTOs;

namespace LibrarySystem.Services.Interfaces
{
    public interface IUserService
    {
    
        Task<List<UserListDto>> ListUsers();
        Task<UserDetailsDto> GetUserDetails(int id);
        Task ApplyUserCreatedEvent(UserCreateDto dto);
        Task ApplyUserUpdatedEvent(int externalUserId, UserUpdateDto dto);
        Task ApplyUserDeactivatedEvent(int externalUserId);

    }
}
