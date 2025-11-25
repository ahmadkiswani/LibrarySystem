
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models
{
    public class Book : AuditLog
    {
        [Key]
        public int Id { get; set; }  

        [Required]
        public string Title { get; set; } = string.Empty;

        public DateTime PublishDate { get; set; }

        [MaxLength(50)]
        public string Version { get; set; } = string.Empty;

        [Required]
        public int AvailableCopies { get; set; }

        [Required]
        public int TotalCopies { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }
        public ICollection<AvailableBook> Copies { get; set; } = new List<AvailableBook>();

    }
}

