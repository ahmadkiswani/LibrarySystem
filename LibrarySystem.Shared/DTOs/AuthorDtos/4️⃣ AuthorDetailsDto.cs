using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using LibrarySystem.Entities.Models;

namespace LibrarySystem.Shared.DTOs
{
    public class AuthorDetailsDto
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string AuthorName { get; set; }
        public int BooksCount { get; set; }
    }
}
