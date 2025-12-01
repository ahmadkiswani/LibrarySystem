using LibrarySystem.DTOs;
using LibrarySystem.DTOs.UserDtos;
using LibrarySystem.DTOs.UserDtos.LibrarySystem.DTOs;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace LibrarySystem.Service
{
    public class UserService
    {
        private readonly LibraryContext _context;
        private int _idCounter = 1;
        private List<User> _users => _context.Users;

        public UserService(LibraryContext context)
        {
            _context = context;
        }

        public void AddUser(UserCreateDto dto)
        {
            User u = new User();
            u.Id = _idCounter++;
            u.UserName = dto.UserName;
            u.UserEmail = dto.UserEmail;
            u.CreatedBy = 1;
            u.CreatedDate = DateTime.Now;
            _users.Add(u);
        }

        public List<UserListDto> ListUsers()
        {
            List<UserListDto> result = new List<UserListDto>();
              return _users
             .Select(u => new UserListDto
             {
                Id = u.Id,
                UserName = u.UserName
                }).ToList();


            return result;
        }

        public UserDetailsDto GetUserById(int id)
        {
            var u = _users.FirstOrDefault(x => x.Id == id);

            if (u == null)
                return null;

            UserDetailsDto dto = new UserDetailsDto();
            dto.Id = u.Id;
            dto.UserName = u.UserName;
            dto.UserEmail = u.UserEmail;

            return dto;
        }

        public void EditUser(int id, UserUpdateDto dto)
        {
            var u = _users.FirstOrDefault(x => x.Id == id);

            if (u != null)
            {
                u.UserName = dto.UserName;
                u.UserEmail = dto.UserEmail;
                u.LastModifiedBy = 1;
                u.LastModifiedDate = DateTime.Now;
            }
        }

        public void DeleteUser(int id)
        {
            var u = _users.FirstOrDefault(x => x.Id == id);

            if (u == null)
                throw new Exception("❗ User not found");

            bool hasActiveBorrow = u.Borrows != null && u.Borrows.Any(b => b.ReturnDate == null);

            if (hasActiveBorrow)
                throw new Exception("❗ User cannot be deleted — user still has borrowed books");

            u.IsDeleted = true;
            u.DeletedBy = id;
            u.DeletedDate = DateTime.Now;   
        
            Console.WriteLine($"User {id} soft deleted");


        }
    }
}