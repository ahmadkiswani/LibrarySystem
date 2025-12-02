
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models
{
 public class Book : AuditLog
 {
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime PublishDate { get; set; }
    public string Version { get; set; }
    public int TotalCopies { get;  set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public ICollection<BookCopy> Copies { get; set; } = new List<BookCopy>();
 }

}

