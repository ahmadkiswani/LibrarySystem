using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using LibrarySystem.Entities.Models;
namespace LibrarySystem.Shared.DTOs.AuthorDtos
{
    public class AuthorUpdateDto
    {
        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string AuthorName { get; set; }
        public int Id { get; set; }

    }
}
