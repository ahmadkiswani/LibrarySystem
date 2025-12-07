using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Shared.DTOs.BookDtos
{
    public class BookListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AvailableCopies { get; set; }
        public string? AuthorName { get; set; }
        public string? CategoryName { get; set; }
        public string? PublisherName { get; set; }
        
    }

}
