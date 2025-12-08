using LibrarySystem.Shared.DTOs.UserDtos;
using LibrarySystem.Shared.DTOs.UserDtos.LibrarySystem.Shared.DTOs;

namespace LibrarySystem.Services.Interfaces
{
    public interface IUserService
    {
        Task AddUser(UserCreateDto dto);
        Task EditUser(int id, UserUpdateDto dto);
        Task DeleteUser(int id,UserDeleteDto dto);
        Task<List<UserListDto>> ListUsers();
    }
}
