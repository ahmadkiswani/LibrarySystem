using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models
{
    public class Author : AuditLog
    {
        [Key]
        public int Id { get; set; }  

        [Required]
        [MaxLength(200)]
        public string AuthorName { get; set; } = string.Empty;

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
