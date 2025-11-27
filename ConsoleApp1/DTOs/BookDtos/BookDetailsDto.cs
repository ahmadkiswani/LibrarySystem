using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.DTOs.BookDtos
{
    public class BookDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public string Version { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public int PublisherId { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
        public string PublisherName { get; set; }

    }

}
