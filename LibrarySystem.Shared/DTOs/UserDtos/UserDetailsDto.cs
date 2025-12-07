
using LibrarySystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Shared.DTOs.UserDtos
{

    namespace LibrarySystem.Shared.DTOs
    {
        public class UserDetailsDto
        {
            public int Id { get; set; }
            public string UserName { get; set; }
            public string UserEmail { get; set; }
            public User? CreatedByUser { get; set; }
            public User? LastModifiedByUser { get; set; }
            public User? DeletedByUser { get; set; }
        }
    }

}
