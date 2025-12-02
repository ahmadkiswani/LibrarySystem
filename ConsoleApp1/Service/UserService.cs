using LibrarySystem.Data;
using LibrarySystem.DTOs.UserDtos;
using LibrarySystem.DTOs.UserDtos.LibrarySystem.DTOs;
using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySystem.Service
{
    public class UserService
    {
        private readonly LibraryDbContext _context;

        public UserService(LibraryDbContext context)
        {
            _context = context;
        }

        public void AddUser(UserCreateDto dto)
        {
            var user = new User
            {
                UserName = dto.UserName,
                UserEmail = dto.UserEmail,
                CreatedBy = 1,
                CreatedDate = DateTime.Now
            };

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public List<UserListDto> ListUsers()
        {
            return _context.Users
                .Where(u => !u.IsDeleted)
                .Select(u => new UserListDto
                {
                    Id = u.Id,
                    UserName = u.UserName
                })
                .ToList();
        }

        public UserDetailsDto GetUserById(int id)
        {
            var u = _context.Users
                .FirstOrDefault(x => x.Id == id && !x.IsDeleted);

            if (u == null)
                return null;

            return new UserDetailsDto
            {
                Id = u.Id,
                UserName = u.UserName,
                UserEmail = u.UserEmail
            };
        }

        public void EditUser(int id, UserUpdateDto dto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id && !u.IsDeleted);

            if (user == null)
                throw new Exception("❗ User not found");

            user.UserName = dto.UserName;
            user.UserEmail = dto.UserEmail;
            user.LastModifiedBy = 1;
            user.LastModifiedDate = DateTime.Now;

            _context.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = _context.Users
                .Include(u => u.Borrows)
                .FirstOrDefault(u => u.Id == id);

            if (user == null)
                throw new Exception("❗ User not found");

            bool hasActiveBorrow = user.Borrows != null &&
                                   user.Borrows.Any(b => b.ReturnDate == null);

            if (hasActiveBorrow)
                throw new Exception("❗ User cannot be deleted — user still has borrowed books");

            user.IsDeleted = true;
            user.DeletedBy = 1;
            user.DeletedDate = DateTime.Now;

            _context.SaveChanges();
        }
    }
}
