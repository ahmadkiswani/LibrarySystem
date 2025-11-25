using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.DTOs.BookDtos
{
    public class BookSearchDto
    {
        public string? Title { get; set; }
        public DateTime? PublishDate { get; set; }
        public string? Version { get; set; }
        public int? AuthorId { get; set; }
        public int? CategoryId { get; set; }
        public int? PublisherId { get; set; }
        public bool? Available { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
