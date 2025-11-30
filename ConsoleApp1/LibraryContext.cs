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
    
    public LibraryContext()
        {
            // authors
            Authors.Add(new Author { Id = 1, AuthorName = "George Orwell" });
            Authors.Add(new Author { Id = 2, AuthorName = "J.K. Rowling" });

            // publisher
            Publishers.Add(new Publisher { Id = 1, Name = "Penguin Books" });

            // category
            Categories.Add(new Category { Id = 1, Name = "Fantasy" });

            // book
            Books.Add(new Book
            {
                Id = 1,
                Title = "1984",
                AuthorId = 1,
                CategoryId = 1,
                PublisherId = 1,
                PublishDate = new DateTime(1949, 6, 8),
                Version = "1st"
            });

            Books.Add(new Book
            {
                Id = 2,
                Title = "Harry Potter",
                AuthorId = 2,
                CategoryId = 1,
                PublisherId = 1,
                PublishDate = new DateTime(1997, 6, 26),
                Version = "1st"
            });

            // Copies
            BookCopies.Add(new BookCopy { Id = 1, BookId = 1, IsAvailable = true });
            BookCopies.Add(new BookCopy { Id = 2, BookId = 2, IsAvailable = true });
        }
    }
}