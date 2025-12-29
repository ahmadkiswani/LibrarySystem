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



        public async Task ApplyUserCreatedEvent(UserCreateDto dto)
        {
            var exists = await _userRepo.GetQueryable()
                .AnyAsync(u => u.ExternalUserId == dto.ExternalUserId);

            if (exists)
                return;

            var user = new User
            {
                ExternalUserId = dto.ExternalUserId,
                UserName = dto.UserName,
                UserEmail = dto.UserEmail,
                UserTypeId = dto.UserTypeId,
            };

            await _userRepo.AddAsync(user);
            await _userRepo.SaveAsync();
        }



        public async Task ApplyUserUpdatedEvent(int externalUserId, UserUpdateDto dto)
        {
            var user = await _userRepo.GetQueryable()
                .FirstOrDefaultAsync(u => u.ExternalUserId == externalUserId);

            if (user == null)
                return;

            user.UserName = dto.UserName;
            user.UserEmail = dto.UserEmail;
            user.UserTypeId = dto.UserTypeId;

            await _userRepo.UpdateAsync(user);
            await _userRepo.SaveAsync();
        }


        public async Task ApplyUserDeactivatedEvent(int externalUserId)
        {
            var user = await _userRepo.GetQueryable()
                .FirstOrDefaultAsync(u => u.ExternalUserId == externalUserId);

            if (user == null)
                return;

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
        public async Task<UserDetailsDto> GetUserDetails(int id)
        {
            var user = await _userRepo.GetQueryable()
                .Include(u => u.CreatedByUser)
                .Include(u => u.LastModifiedByUser)
                .Include(u => u.DeletedByUser)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new Exception("User not found");

            return new UserDetailsDto
            {
                Id = user.Id,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                CreatedByUser = user.CreatedByUser,
                LastModifiedByUser = user.LastModifiedByUser,
                DeletedByUser = user.DeletedByUser
            };
        }
    }
}