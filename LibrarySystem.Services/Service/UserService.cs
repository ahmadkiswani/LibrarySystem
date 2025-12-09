using LibrarySystem.Domain.Repositories;
using LibrarySystem.Entities.Models;
using LibrarySystem.Services.Interfaces;
using LibrarySystem.Shared.DTOs.UserDtos;
using LibrarySystem.Shared.DTOs.UserDtos.LibrarySystem.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<UserType> _userTypeRepo;

        public UserService(
            IGenericRepository<User> userRepo,
            IGenericRepository<UserType> userTypeRepo)
        {
            _userRepo = userRepo;
            _userTypeRepo = userTypeRepo;
        }


        public async Task AddUser(UserCreateDto dto)
        {
            bool emailExists = await _userRepo.GetQueryable()
                .AnyAsync(u => u.UserEmail == dto.UserEmail);

            if (emailExists)
                throw new Exception("Email already exists");

            bool usernameExists = await _userRepo.GetQueryable()
                .AnyAsync(u => u.UserName == dto.UserName);

            if (usernameExists)
                throw new Exception("Username already exists");

            bool typeExists = await _userTypeRepo.GetQueryable()
                .AnyAsync(t => t.Id == dto.UserTypeId);

            if (!typeExists)
                throw new Exception("UserType does not exist");

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
            if (user == null)
                throw new Exception("User not found");

            bool emailExists = await _userRepo.GetQueryable()
                .AnyAsync(u => u.UserEmail == dto.UserEmail && u.Id != id);

            if (emailExists)
                throw new Exception("Email already exists for another user");

            bool usernameExists = await _userRepo.GetQueryable()
                .AnyAsync(u => u.UserName == dto.UserName && u.Id != id);

            if (usernameExists)
                throw new Exception("Username already exists for another user");

            bool typeExists = await _userTypeRepo.GetQueryable()
                .AnyAsync(t => t.Id == dto.UserTypeId);

            if (!typeExists)
                throw new Exception("UserType does not exist");

            bool isLastAdmin = false;

            if (user.UserTypeId == 1 && dto.UserTypeId != 1)
            {
                int adminCount = await _userRepo.GetQueryable()
                    .CountAsync(u => u.UserTypeId == 1);

                if (adminCount == 1)
                    throw new Exception("Cannot remove the last admin");
            }

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

            if (user.UserTypeId == 1)
            {
                int adminCount = await _userRepo.GetQueryable()
                    .CountAsync(u => u.UserTypeId == 1);

                if (adminCount == 1)
                    throw new Exception("Cannot delete the last admin");
            }

            await _userRepo.SoftDeleteAsync(user);
            await _userRepo.SaveAsync();
        }


        public async Task<List<UserListDto>> ListUsers()
        {
            var users = await _userRepo.FindAsync(
                u => true,
                q => q.Include(u => u.UserType)
            );

            return users.Select(u => new UserListDto
            {
                Id = u.Id,
                UserName = u.UserName,
                UserTypeId = u.UserTypeId,
                UserTypeName = u.UserType != null ? u.UserType.TypeName : "Unknown"
            }).ToList();
        }


    }
}