using LibrarySystem.Data;  
using LibrarySystem.DTOs;
using LibrarySystem.DTOs.BookDtos;
using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySystem.Service
{
    public class BookService
    {
        private readonly LibraryDbContext _context;
        private readonly BookCopyService _bookCopyService;

        public BookService(LibraryDbContext context, BookCopyService bookCopyService)
        {
            _context = context;
            _bookCopyService = bookCopyService;
        }

        public void AddBook(BookCreateDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                PublishDate = dto.PublishDate,
                Version = dto.Version,
                AuthorId = dto.AuthorId,
                CategoryId = dto.CategoryId,
                CreatedBy = 1,
                CreatedDate = DateTime.Now
            };

            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public List<BookListDto> GetBooksByAuthor(int authorId)
        {
            return _context.Books
                .Where(b => b.AuthorId == authorId && !b.IsDeleted)
                .Select(b => new BookListDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorName = b.Author.AuthorName
                })
                .ToList();
        }

        public List<BookListDto> GetBooksByCategoryId(int categoryId)
        {
            return _context.Books
                .Where(b => b.CategoryId == categoryId && !b.IsDeleted)
                .Select(b => new BookListDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorName = b.Author.AuthorName,
                    CategoryName = b.Category.Name,
                })
                .ToList();
        }

        public List<BookListDto> GetAllBooks()
        {
            return _context.Books
                .Where(b => !b.IsDeleted)
                .Select(b => new BookListDto
                {
                    Id = b.Id,
                    Title = b.Title
                })
                .ToList();
        }

        public BookDetailsDto GetBookById(int id)
        {
            var book = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Copies)
                .FirstOrDefault(b => b.Id == id && !b.IsDeleted);

            if (book == null)
                throw new Exception("Book not found");

            return new BookDetailsDto
            {
                Id = book.Id,
                Title = book.Title,
                PublishDate = book.PublishDate,
                Version = book.Version,
                AuthorId = book.AuthorId,
                CategoryId = book.CategoryId,
                AuthorName = book.Author.AuthorName,
                CategoryName = book.Category.Name,
                TotalCopies = book.TotalCopies
            };
        }

        public void EditBook(int id, BookUpdateDto dto)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id && !b.IsDeleted);

            if (book == null)
                throw new Exception("Book not found");

            book.Title = dto.Title;
            book.PublishDate = dto.PublishDate;
            book.Version = dto.Version;
            book.AuthorId = dto.AuthorId;
            book.CategoryId = dto.CategoryId;
            book.LastModifiedBy = 1;
            book.LastModifiedDate = DateTime.Now;

            _context.SaveChanges();
        }

        public void DeleteBook(int id)
        {
            var book = _context.Books
                .Include(b => b.Copies)
                .ThenInclude(c => c.BorrowRecords)
                .FirstOrDefault(b => b.Id == id);

            if (book == null)
                throw new Exception("❗ Book not found");

            bool hasActiveBorrow = book.Copies.Any(copy => copy.BorrowRecords.Any(br => br.ReturnDate == null));

            if (hasActiveBorrow)
                throw new Exception("❗ Cannot delete book — copies are currently borrowed");

            book.IsDeleted = true;
            book.DeletedDate = DateTime.Now;
            book.DeletedBy = 1;

            _context.SaveChanges();
        }

        public List<BookListDto> Search(BookSearchDto dto)
        {
            int page = dto.Page <= 0 ? 1 : dto.Page;
            int pageSize = dto.PageSize <= 0 || dto.PageSize > 200 ? 10 : dto.PageSize;
            string text = dto.Text?.Trim().ToLower() ?? "";

            return _context.Books
                .Where(b => !b.IsDeleted)
                .Where(b =>
                    (dto.Text == null || b.Title.ToLower().Contains(dto.Text.ToLower())) &&
                    (dto.Version == null || b.Version.ToLower() == dto.Version.ToLower()) &&
                    (dto.PublishDate == null || b.PublishDate.Date == dto.PublishDate.Value.Date) &&
                    (dto.AuthorId == null || b.AuthorId == dto.AuthorId) &&
                    (dto.CategoryId == null || b.CategoryId == dto.CategoryId) &&
                    (dto.Number == null || b.Id == dto.Number)
                )
                .OrderBy(b => b.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BookListDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorName = b.Author.AuthorName,
                    CategoryName = b.Category.Name
                })
                .ToList();
        }
    }
}
