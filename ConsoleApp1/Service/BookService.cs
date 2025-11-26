using LibrarySystem.DTOs;
using LibrarySystem.DTOs.BookDtos;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySystem.Service
{
    public class BookService
    {
        private readonly List<Book> _books;
        private readonly AvailableBookService _availableBookService;
        private int _idCounter = 1;


        public BookService(List<Book> books, AvailableBookService availableBookService)
        {
            _books = books;
            _availableBookService = availableBookService;
        }

        public void AddBook(BookCreateDto dto)
        {
            Book book = new Book();
            book.Id = _idCounter++;
            book.Title = dto.Title;
            book.PublishDate = dto.PublishDate;
            book.Version = dto.Version;
            book.AuthorId = dto.AuthorId;
            book.CategoryId = dto.CategoryId;
            book.PublisherId = dto.PublisherId;
            book.TotalCopies = dto.TotalCopies;

            book.CreatedBy = 1;
            book.CreatedDate = DateTime.Now;

            _books.Add(book);
        }

        public List<BookListDto> GetAllBooks()
        {
            List<BookListDto> list = new List<BookListDto>();

                    return _books
             .Select(book => new BookListDto
             {
                 Id = book.Id,
                 Title = book.Title
             }).ToList();

        }

        public BookDetailsDto GetBookById(int id)
        {
            Book book = _books.FirstOrDefault(b => b.Id == id);

            if (book == null)
                return null;

            BookDetailsDto dto = new BookDetailsDto();

            dto.Id = book.Id;
            dto.Title = book.Title;
            dto.PublishDate = book.PublishDate;
            dto.Version = book.Version;
            dto.AuthorId = book.AuthorId;
            dto.CategoryId = book.CategoryId;
            dto.PublisherId = book.PublisherId;
            dto.TotalCopies = book.TotalCopies;

            return dto;
        }

        public void EditBook(int id, BookUpdateDto dto)
        {
            Book book = _books.FirstOrDefault(b => b.Id == id);

            if (book != null)
            {
                book.Title = dto.Title;
                book.PublishDate = dto.PublishDate;
                book.Version = dto.Version;
                book.AuthorId = dto.AuthorId;
                book.CategoryId = dto.CategoryId;
                book.PublisherId = dto.PublisherId;
                book.TotalCopies = dto.TotalCopies;

                book.LastModifiedBy = 1;
                book.LastModifiedDate = DateTime.Now;
            }
        }

        public void DeleteBook(int id)
        {
            Book book = _books.FirstOrDefault(b => b.Id == id);

            if (book != null)
            {
                _books.Remove(book);
            }
        }

        public int GetTotalCopies(int bookId)
        {
            return _availableBookService.GetAllCopiesForBook(bookId).Count;
        }

        public int GetAvailableCopies(int bookId)
        {
            return _availableBookService.GetAvailableCount(bookId);
        }

        public int GetBorrowedCopies(int bookId)
        {
            return _availableBookService.GetBorrowedCount(bookId);
        }

        public List<BookListDto> SearchBooks(BookSearchDto dto)
        {
            var filtered = _books;

            if (!string.IsNullOrEmpty(dto.Title))
                filtered = filtered.Where(b => b.Title.ToLower().Contains(dto.Title.ToLower())).ToList();

            if (dto.PublishDate.HasValue)
                filtered = filtered.Where(b => b.PublishDate == dto.PublishDate.Value).ToList();

            if (!string.IsNullOrEmpty(dto.Version))
                filtered = filtered.Where(b => b.Version.ToLower() == dto.Version.ToLower()).ToList();

            if (dto.AuthorId.HasValue)
                filtered = filtered.Where(b => b.AuthorId == dto.AuthorId.Value).ToList();

            if (dto.CategoryId.HasValue)
                filtered = filtered.Where(b => b.CategoryId == dto.CategoryId.Value).ToList();

            if (dto.PublisherId.HasValue)
                filtered = filtered.Where(b => b.PublisherId == dto.PublisherId.Value).ToList();

            if (dto.Available.HasValue)
            {
                if (dto.Available.Value == true)
                    filtered = filtered.Where(b => _availableBookService.GetAvailableCount(b.Id) > 0).ToList();
                else
                    filtered = filtered.Where(b => _availableBookService.GetAvailableCount(b.Id) == 0).ToList();
            }

            
            int skip = (dto.Page - 1) * dto.PageSize;
            filtered = filtered.Skip(skip).Take(dto.PageSize).ToList();

            return filtered.Select(b => new BookListDto
            {
                Id = b.Id,
                Title = b.Title
            }).ToList();
        }

    }
}