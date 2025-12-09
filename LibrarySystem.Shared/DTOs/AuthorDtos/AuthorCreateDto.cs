using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using LibrarySystem.Entities.Models;


namespace LibrarySystem.Shared.DTOs
{
    public class AuthorCreateDto
    {
        [Required(ErrorMessage = "Author name is required")]
        [StringLength(60, MinimumLength = 3)]
        public string AuthorName { get; set; } = string.Empty;
    }
}
