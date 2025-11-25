using System;
using System.Collections.Generic;

namespace LibrarySystem.Models
{
    public class AvailableBook : AuditLog
    {
        public int Id { get; set; }

        public bool IsAvailable { get; set; } = true;

        public string CopyCode { get; set; } = string.Empty;

        public int BookId { get; set; }
        public Book Book { get; set; }

        public List<Borrow> BorrowRecords { get; set; } = new();
    }
}
