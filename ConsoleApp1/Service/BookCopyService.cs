using LibrarySystem.DTOs;
using LibrarySystem.DTOs.AvailableBookDto;
using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using LibrarySystem.Data;

namespace LibrarySystem.Service
{
    public class BookCopyService
    {

        private readonly LibraryDbContext _context;

        public BookCopyService(LibraryDbContext context)
        {
            _context = context;
        }
        public void AddBookCopy(BookCopyCreateDto dto)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == dto.BookId && !b.IsDeleted);

            if (book == null)
                throw new Exception($"❗ Book with Id {dto.BookId} does NOT exist");

            var copy = new BookCopy
            {
                BookId = dto.BookId,
                CreatedBy = 1,
                CreatedDate = DateTime.Now
            };

            _context.BookCopies.Add(copy);
            _context.SaveChanges();
        }



        public void DeleteBookCopy(int id)
        {
            var copy = _context.BookCopies
             .Include(c => c.BorrowRecords)
             .FirstOrDefault(x => x.Id == id && !x.IsDeleted); 
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
            return _context.BookCopies
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
            return _context.BookCopies.Count(x => x.BookId == bookId && x.IsAvailable && !x.IsDeleted);

        }

        public int GetBorrowedCount(int bookId)
            {
            return _context.BookCopies.Count(x => x.BookId == bookId && !x.IsAvailable && !x.IsDeleted);
            }

        public List<BookCopy> GetAllCopiesForBook(int bookId)
        {
            return _context.BookCopies.Where(x => x.BookId == bookId && !x.IsDeleted).ToList();
        }

        public BookCopy GetSpecificCopy(int availableBookId)
        {
            var copy= _context.BookCopies.FirstOrDefault(x => x.Id == availableBookId && !x.IsDeleted);

            if (copy == null)
                throw new Exception("Copy not found");

            return copy;

        }
        public int GetTotalCopies(int bookId)
        {
            return _context.BookCopies.Count(x => x.BookId == bookId && !x.IsDeleted);
        }   

        public int GetAvailableCopies(int bookId)
        {
            return _context.BookCopies.Count(x => x.BookId == bookId && x.IsAvailable && !x.IsDeleted);
        }

        public int GetBorrowedCopies(int bookId)
        {
            return _context.BookCopies.Count(x => x.BookId == bookId && !x.IsAvailable && !x.IsDeleted);
        }

    }
}
