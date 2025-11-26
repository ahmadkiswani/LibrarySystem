using LibrarySystem.DTOs;
using LibrarySystem.DTOs.AvailableBookDto;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySystem.Service
{
    public class BookCopySservice
    {
        private List<BookCopy> _inventory;
        private List<Book> _books;

        private int _idCounter = 1;

        public BookCopySservice(List<BookCopy> inventory, List<Book> books)
        {
            _inventory = inventory;
            _books = books;
        }
        public void AddAvailableBook(AvailableBookCreateDto dto)
        {
            BookCopy a = new BookCopy();
            a.Id = _idCounter++;
            a.BookId = dto.BookId;
            a.CreatedBy = 1;
            a.CreatedDate = DateTime.Now;
            _inventory.Add(a);
            var book = _books.FirstOrDefault(b => b.Id == dto.BookId);

            if (book != null)
            {
                if (book.Copies == null)
                    book.Copies = new List<BookCopy>();

                book.Copies.Add(a);
            }

        }

        public List<AvailableBookListDto> ListAvailableBooks()
        {
            List<AvailableBookListDto> result = new List<AvailableBookListDto>();

            return _inventory
             .Select(a => new AvailableBookListDto
             {
                    Id = a.Id,
                    BookId = a.BookId,
                    IsAvailable = a.IsAvailable
                }).ToList();
        }
            
            
            public int GetAvailableCount(int bookId)
            {
                return _inventory.Count(x => x.BookId == bookId && x.IsAvailable);
            }

            public int GetBorrowedCount(int bookId)
            {
                return _inventory.Count(x => x.BookId == bookId && !x.IsAvailable);
            }

            public List<BookCopy> GetAllCopiesForBook(int bookId)
            {
                return _inventory.Where(x => x.BookId == bookId).ToList();
            }

            public BookCopy GetSpecificCopy(int availableBookId)
            {
                return _inventory.FirstOrDefault(x => x.Id == availableBookId);
            }
        }
}
