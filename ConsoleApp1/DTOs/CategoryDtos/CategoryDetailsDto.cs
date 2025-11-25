using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.DTOs
{
    public class CategoryDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BooksCount { get; set; }
    }
}

