using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models
{
    public class BookCopy : AuditLog
    {
        [Required]
        public int Id { get; set; }

        public bool IsAvailable { get; set; } = true;
        [Required]
        [MaxLength(50)]
        public string CopyCode { get; set; } = string.Empty;

        public int BookId { get; set; }
        public Book Book { get; set; }

        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        public ICollection<Borrow> BorrowRecords { get; set; } = new List<Borrow>();
    }
}
