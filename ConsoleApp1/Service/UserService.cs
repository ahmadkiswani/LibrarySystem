using LibrarySystem.DTOs.UserDtos;
using LibrarySystem.DTOs.UserDtos.LibrarySystem.DTOs;
using LibrarySystem.Models;
using LibrarySystem.Repository;

namespace LibrarySystem.Service
{
    public class UserService
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
                CreatedBy = dto.CreatedByUser.Id, 
                CreatedDate = DateTime.Now,
                IsDeleted = false
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
            user.LastModifiedBy = dto.LastModifiedByUser.Id;
            user.LastModifiedDate = DateTime.Now;

            await _userRepo.Update(user);
            await _userRepo.SaveAsync();
        }
        public async Task DeleteUser(int id, UserUpdateDto dto)
        {
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
                throw new Exception("User not found");
            user.IsDeleted = true;
            user.DeletedBy =dto.DeletedByUser.Id;
            user.DeletedDate = DateTime.Now;

            await _userRepo.Update(user);
            await _userRepo.SaveAsync();
        }
        public async Task<List<UserListDto>> ListUsers()
        {
            var users = await _userRepo.FindAsync(u => !u.IsDeleted);
            return users.Select(u => new UserListDto
            {
                Id = u.Id,
                UserName = u.UserName
            }).ToList();
        }
    }
}