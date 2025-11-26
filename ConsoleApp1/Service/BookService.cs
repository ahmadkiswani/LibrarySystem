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
        private readonly BookCopySservice _bookCopyService;
        private int _idCounter = 1;


        public BookService(List<Book> books, BookCopySservice bookCopyService)
        {
            _books = books;
            _bookCopyService = bookCopyService;
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
            return _bookCopyService.GetAllCopiesForBook(bookId).Count;
        }

        public int GetAvailableCopies(int bookId)
        {
            return _bookCopyService.GetAvailableCount(bookId);
        }

        public int GetBorrowedCopies(int bookId)
        {
            return _bookCopyService.GetBorrowedCount(bookId);
        }

        public List<BookListDto> SearchBooks(BookSearchDto dto)
        {
            
            var query = _books.AsQueryable();

           
            if (!string.IsNullOrWhiteSpace(dto.Title))
            {
                var title = dto.Title.ToLower();
                query = query.Where(b => b.Title.ToLower().Contains(title));
            }

            if (dto.PublishDate.HasValue)
            {
                var date = dto.PublishDate.Value.Date;
                query = query.Where(b => b.PublishDate.Date == date);
            }

            if (!string.IsNullOrWhiteSpace(dto.Version))
            {
                var version = dto.Version.ToLower();
                query = query.Where(b => b.Version.ToLower() == version);
            }

            if (dto.AuthorId.HasValue)
            {
                int authorId = dto.AuthorId.Value;
                query = query.Where(b => b.AuthorId == authorId);
            }

            if (dto.CategoryId.HasValue)
            {
                int categoryId = dto.CategoryId.Value;
                query = query.Where(b => b.CategoryId == categoryId);
            }
          
            if (dto.Available.HasValue)
            {
                bool mustBeAvailable = dto.Available.Value;

                query = query.Where(b =>
                    mustBeAvailable
                        ? _bookCopyService.GetAvailableCount(b.Id) > 0
                        : _bookCopyService.GetAvailableCount(b.Id) == 0
                );
            }

          
            int page = dto.Page <= 0 ? 1 : dto.Page;
            int pageSize = dto.PageSize <= 0 ? 10 : dto.PageSize;

            int skip = (page - 1) * pageSize;

            query = query
                .Skip(skip)
                .Take(pageSize);

           
            return query
                .Select(b => new BookListDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    
                })
                .ToList();
        }
    }

}
