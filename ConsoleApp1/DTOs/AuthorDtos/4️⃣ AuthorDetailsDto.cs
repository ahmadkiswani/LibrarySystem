using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
namespace LibrarySystem.DTOs
{
    public class AuthorDetailsDto
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string AuthorName { get; set; }
        public int BooksCount { get; set; }
    }
}
