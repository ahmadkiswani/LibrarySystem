using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibrarySystem.Shared.DTOs.UserDtos
{
    public class UserUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(120)]
        public string UserEmail { get; set; } = string.Empty;

        public int LastModifiedBy { get; set; } = 1;
        public int? UserTypeId { get; set; }
    }

}
