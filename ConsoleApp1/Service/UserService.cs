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
        private List<User> _users;
        private int _idCounter = 1;

        public UserService(List<User> users)
        {
            _users = users;
        }

        public void AddUser(UserCreateDto dto)
        {
            User u = new User();
            u.Id = _idCounter++;
            u.UserName = dto.UserName;
            u.UserEmail = dto.UserEmail;
            u.PasswordHash = dto.Password;  
            u.CreatedBy = 1;
            u.CreatedDate = DateTime.Now;
            _users.Add(u);
        }

        public List<UserListDto> ListUsers()
        {
            List<UserListDto> result = new List<UserListDto>();

            foreach (var u in _users)
            {
                UserListDto dto = new UserListDto();
                dto.Id = u.Id;
                dto.UserName = u.UserName;
                result.Add(dto);
            }

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

            if (u != null)
            {
                _users.Remove(u);
            }
        }
    }
}
