using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Shared.DTOs.UserDtos
{
    namespace LibrarySystem.Shared.DTOs
    {
        public class UserListDto
        {
            public int Id { get; set; }
            public string UserName { get; set; }= string.Empty; 
            public int? UserTypeId { get; set; }
            public string? UserTypeName { get; set; }
        }
    }

}
