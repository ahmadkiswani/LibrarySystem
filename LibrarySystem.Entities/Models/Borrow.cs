using System;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Entities.Models
{
    public class Borrow : AuditLog
    {
        [Required]
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int BookCopyId { get; set; }
        public BookCopy BookCopy { get; set; }

        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsOverdue { get; set; } = false;
        public int? OverdueDays { get; set; } = 0;

    }
}
