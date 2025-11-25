using System;
using System.Collections.Generic;
using System.Text;
namespace LibrarySystem.DTOs
{
    public class AuthorDetailsDto
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public int BooksCount { get; set; }
    }
}
