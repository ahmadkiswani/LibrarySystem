using LibrarySystem.Domain.Repositories;
using LibrarySystem.Entities.Models;
using LibrarySystem.Services.Interfaces;
using LibrarySystem.Shared.DTOs.UserDtos;
using LibrarySystem.Shared.DTOs.UserDtos.LibrarySystem.Shared.DTOs;

namespace LibrarySystem.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepo;

        public UserService(IGenericRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task AddUser(UserCreateDto dto)
        {
            var user = new User
            {
                UserName = dto.UserName,
                UserEmail = dto.UserEmail,
                UserTypeId = dto.UserTypeId
            };

            await _userRepo.AddAsync(user);
            await _userRepo.SaveAsync();
        }

        public async Task EditUser(int id, UserUpdateDto dto)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null || user.IsDeleted)
                throw new Exception("User not found");

            user.UserName = dto.UserName;
            user.UserEmail = dto.UserEmail;
            user.UserTypeId = dto.UserTypeId;

            await _userRepo.UpdateAsync(user);
            await _userRepo.SaveAsync();
        }

        public async Task DeleteUser(int id, UserDeleteDto dto)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
                throw new Exception("User not found");

            await _userRepo.SoftDeleteAsync(user);
            await _userRepo.SaveAsync();
        }

        public async Task<List<UserListDto>> ListUsers()
        {
            var users = await _userRepo.FindAsync(u => !u.IsDeleted);

            return users.Select(u => new UserListDto
            {
                Id = u.Id,
                UserName = u.UserName,
            }).ToList();
        }
    }
}
