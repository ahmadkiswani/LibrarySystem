using System;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Shared.DTOs.UserDtos
{
    public class UserCreateDto
    {
        [Required]
        [StringLength(40, MinimumLength = 2)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(120)]
        public string UserEmail { get; set; } = string.Empty;

        public int CreatedBy { get; set; } = 1;
    }
}
