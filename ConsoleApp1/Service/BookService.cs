using LibrarySystem.DTOs;
using LibrarySystem.DTOs.AvailableBookDto;
using LibrarySystem.DTOs.BookDtos;
using LibrarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySystem.Service
{
    public class BookService
    {
        private readonly LibraryContext _context;

        private List<Book> _books => _context.Books;
        private List<Author> _authors => _context.Authors;
        private List<Category> _categories => _context.Categories;
        private List<Publisher> _publishers => _context.Publishers;

        private readonly BookCopyService _bookCopyService;
        private int _idCounter = 1;
        public BookService(LibraryContext context, BookCopyService bookCopyService)
        {
            _context = context;
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
        public List<BookListDto> GetBooksByAuthor(int authorId)
        {
            var result =
                from book in _books
                join author in _authors on book.AuthorId equals author.Id
                where author.Id == authorId && !book.IsDeleted
                select new BookListDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    AuthorName = author.AuthorName
                };

            return result.ToList();
        }
        public List<BookListDto> GetBooksByCategoryId(int categoryId)
        {
            var result =
                from book in _books
                join category in _categories on book.CategoryId equals category.Id
                where category.Id == categoryId && !book.IsDeleted
                select new BookListDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    AuthorName = _authors.FirstOrDefault(a => a.Id == book.AuthorId)?.AuthorName,
                    CategoryName = category.Name,
                    PublisherName = _publishers.FirstOrDefault(p => p.Id == book.PublisherId)?.Name
                };

            return result.ToList();
        }


        public List<BookListDto> GetAllBooks()
        {
            List<BookListDto> list = new List<BookListDto>();
            return _books
                .Where(b => !b.IsDeleted)
                .Select(book => new BookListDto
                {
                 Id = book.Id,
                 Title = book.Title
                })
                 .ToList();

        }
        public BookDetailsDto GetBookById(int id)
        {
            Book book = _books.FirstOrDefault(b => b.Id == id && !b.IsDeleted);
            BookDetailsDto dto = new BookDetailsDto();
            dto.Id = book.Id;
            dto.Title = book.Title;
            dto.PublishDate = book.PublishDate;
            dto.Version = book.Version;
            dto.AuthorId = book.AuthorId;
            dto.CategoryId = book.CategoryId;
            dto.TotalCopies = book.TotalCopies;
            dto.AuthorName = book.Author != null && !book.Author.IsDeleted
                ? book.Author.AuthorName
                : "Unknown";

            dto.CategoryName = book.Category != null && !book.Category.IsDeleted
                ? book.Category.Name
                : "Unknown";

            dto.PublisherName = book.Publisher != null && !book.Publisher.IsDeleted
                ? book.Publisher.Name
                : "Unknown";


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
            var book = _books.FirstOrDefault(b => b.Id == id);

            if (book == null)
                throw new Exception("❗ Book not found");

            bool hasActiveBorrow = book.Copies.Any(copy => copy.BorrowRecords.Any(bc => bc.ReturnDate == null));
            if (hasActiveBorrow)
                throw new Exception("❗ Cannot delete book — copies are currently borrowed");

            book.IsDeleted = true;
            book.DeletedDate = DateTime.Now;
            book.DeletedBy = id;
            Console.WriteLine($"book {id} soft deleted");

            bool allCopiesDeleted = book.Copies.All(c => c.IsDeleted);
            hasActiveBorrow = book.Copies.Any(c => c.BorrowRecords.Any(b => b.ReturnDate == null));

            if (allCopiesDeleted && !hasActiveBorrow)
            {
                book.IsDeleted = true;
                book.LastModifiedBy = 1;
                book.LastModifiedDate = DateTime.Now;
                Console.WriteLine($"Book {book.Id} auto-soft-deleted because all copies are deleted");
            }
        }



        public int GetTotalCopies(int bookId)
        {
            return _bookCopyService.GetAllCopiesForBook(bookId).Count;
        }

        public int BookCopyService(int bookId)
        {
            return _bookCopyService.GetAvailableCount(bookId);
        }

        public int GetBorrowedCopies(int bookId)
        {
            return _bookCopyService.GetBorrowedCount(bookId);
        }
        public List<BookListDto> GetBooksByPublisher(int publisherId)
        {
            var result =
                from book in _books
                join publisher in _publishers on book.PublisherId equals publisher.Id
                where publisher.Id == publisherId && !book.IsDeleted
                select new BookListDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    AuthorName = _authors.FirstOrDefault(a => a.Id == book.AuthorId)?.AuthorName,
                    CategoryName = _categories.FirstOrDefault(c => c.Id == book.CategoryId)?.Name,
                    PublisherName = publisher.Name
                };

            return result.ToList();
        }


        public List<BookListDto> Search(BookSearchDto dto)
        {
            return _books
                .Where(b => !b.IsDeleted)
                .Where(b =>
                    (dto.Text == null || b.Title.ToLower().Contains(dto.Text.ToLower())) &&
                    (dto.Version == null || b.Version.ToLower() == dto.Version.ToLower()) &&
                    (dto.PublishDate == null || b.PublishDate.Date == dto.PublishDate.Value.Date) &&
                    (dto.AuthorId == null || b.AuthorId == dto.AuthorId) &&
                    (dto.CategoryId == null || b.CategoryId == dto.CategoryId) &&
                    (dto.PublisherId == null || b.PublisherId == dto.PublisherId) &&
                    (dto.Number == null || b.Id == dto.Number) &&        
                    (dto.Available == null ||
                        (dto.Available.Value
                            ? b.Copies.Any(c => c.IsAvailable && !c.IsDeleted)
                            : !b.Copies.Any(c => c.IsAvailable && !c.IsDeleted)
                        )
                    )
                )
                .OrderBy(b => b.Title) 
                .Skip((dto.Page - 1) * dto.PageSize)
                .Take(dto.PageSize)
                .Select(b => new BookListDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorName = _context.Authors.FirstOrDefault(a => a.Id == b.AuthorId)?.AuthorName,
                    CategoryName = _context.Categories.FirstOrDefault(c => c.Id == b.CategoryId)?.Name,
                    PublisherName = _context.Publishers.FirstOrDefault(p => p.Id == b.PublisherId)?.Name
                })
                .ToList();
        }


    }

}
