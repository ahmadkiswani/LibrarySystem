using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibrarySystem.DTOs.UserDtos
{
    public class UserUpdateDto
    {
        [Required]
        [StringLength(40)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(120)]
        public string UserEmail { get; set; }

        [Required]
        public int LastModifiedBy { get; set; }
    }

}
