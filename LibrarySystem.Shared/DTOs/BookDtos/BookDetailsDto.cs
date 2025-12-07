using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Shared.DTOs.BookDtos
{
    public class BookDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }
        public DateTime PublishDate { get; set; }

        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
        public string PublisherName { get; set; }

        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public int BorrowedCopies { get; set; }

        public DateTime? LastBorrowedDate { get; set; }

        public bool IsDeleted { get; set; }
    }


}
