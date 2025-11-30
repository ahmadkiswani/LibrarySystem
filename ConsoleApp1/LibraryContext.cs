using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

using LibrarySystem.Models;

namespace LibrarySystem
{
    public class LibraryContext
    {
        public List<Author> Authors { get; set; } = new();
        public List<Book> Books { get; set; } = new();
        public List<BookCopy> BookCopies { get; set; } = new();
        public List<Borrow> Borrows { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public List<Publisher> Publishers { get; set; } = new();
        public List<User> Users { get; set; } = new();
    }
}
