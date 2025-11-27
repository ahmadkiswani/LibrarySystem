using LibrarySystem.DTOs;
using LibrarySystem.DTOs.AvailableBookDto;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySystem.Service
{
    public class BookCopyService
    {
        private List<BookCopy> _inventory;
        private List<Book> _books;

        private int _idCounter = 1;

        public BookCopyService(List<BookCopy> inventory, List<Book> books)
        {
            _inventory = inventory;
            _books = books;
        }
        public void AddBookCopy(BookCopyCreateDto dto)
        {
            var book = _books.FirstOrDefault(b => b.Id == dto.BookId);

            if (book == null)
                throw new Exception($"❗ Book with Id {dto.BookId} does NOT exist");

            BookCopy a = new BookCopy();
            a.Id = _idCounter++;
            a.BookId = dto.BookId;
            a.CreatedBy = 1;
            a.CreatedDate = DateTime.Now;

            _inventory.Add(a);
            book.Copies.Add(a);
        }
        public void DeleteBookCopy(int id)
        {
            var copy = _inventory.FirstOrDefault(x => x.Id == id&&!x.IsDeleted);

            if (copy == null)
                throw new Exception("Copy not found");

            bool isBorrowedActive = copy.BorrowRecords.Any(b => b.ReturnDate == null);
            if (isBorrowedActive)
                throw new Exception($"Cannot delete copy {id} — it is currently borrowed");
            copy.IsDeleted = true;
            copy.IsAvailable = false;
            copy.LastModifiedBy = 0;
            copy.LastModifiedDate = DateTime.Now;

            Console.WriteLine($"Copy {id} marked as deleted (soft delete)");
        }

        public List<BookCopyListDto> ListBookCopies()
        {
            return _inventory
            .Where(copy => !copy.IsDeleted)

             .Select(copy => new BookCopyListDto
             {
                 Id = copy.Id,
                 BookId = copy.BookId,
                 IsAvailable = copy.IsAvailable
             }).ToList();
        }

        public int GetAvailableCount(int bookId)
            {
            return _inventory.Count(x => x.BookId == bookId && x.IsAvailable && !x.IsDeleted);

        }

        public int GetBorrowedCount(int bookId)
            {
                return _inventory.Count(x => x.BookId == bookId && !x.IsAvailable && !x.IsDeleted);
            }

            public List<BookCopy> GetAllCopiesForBook(int bookId)
            {
                return _inventory.Where(x => x.BookId == bookId && !x.IsDeleted).ToList();
            }

            public BookCopy GetSpecificCopy(int availableBookId)
            {
                return _inventory.FirstOrDefault(x => x.Id == availableBookId && !x.IsDeleted);
            }
        }
}
