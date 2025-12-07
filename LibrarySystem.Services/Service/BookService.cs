using LibrarySystem.Entities.Models;
using LibrarySystem.Domain.Data;
using LibrarySystem.Domain.Repositories;

using LibrarySystem.Shared.DTOs.AuthorDtos;
using LibrarySystem.Shared.DTOs.BookDtos;


using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Service
{
    public class BookService
    {
        private readonly IGenericRepository<Book> _bookRepo;
        private readonly IGenericRepository<BookCopy> _copyRepo;
        private readonly IGenericRepository<Borrow> _borrowRepo;

        public BookService(
            IGenericRepository<Book> bookRepo,
            IGenericRepository<BookCopy> copyRepo,
            IGenericRepository<Borrow> borrowRepo)
        {
            _bookRepo = bookRepo;
            _copyRepo = copyRepo;
            _borrowRepo = borrowRepo;
        }
        public async Task<int> AddBook(BookCreateDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                PublishDate = dto.PublishDate,
                Version = dto.Version,
                AuthorId = dto.AuthorId,
                CategoryId = dto.CategoryId,
                PublisherId = dto.PublisherId,
                CreatedBy = 0,
                CreatedDate = DateTime.Now
            };

            await _bookRepo.AddAsync(book);
            await _bookRepo.SaveAsync();
            return book.Id;
        }

      
        public async Task<List<BookListDto>> GetAllBooks()
        {
            var books = await _bookRepo.FindAsync(b => !b.IsDeleted);

            return books.Select(b => new BookListDto
            {
                Id = b.Id,
                Title = b.Title
            }).ToList();
        }


        public async Task<BookDetailsDto> GetBookById(int id)
        {
            var book = (await _bookRepo.FindAsync(
                b => b.Id == id && !b.IsDeleted,
                include: q => q
                    .Include(b => b.Author)
                    .Include(b => b.Category)
                    .Include(b => b.Publisher)
            )).FirstOrDefault();

            if (book == null)
                throw new Exception("Book not found");

            var copies = await _copyRepo.FindAsync(c => c.BookId == id && !c.IsDeleted);
            int available = copies.Count(c => c.IsAvailable);
            int borrowed = copies.Count(c => !c.IsAvailable);

            var borrowRecords = await _borrowRepo.FindAsync(
                b => b.BookCopy.BookId == id,
                include: q => q.Include(b => b.BookCopy)
            );

            DateTime? lastBorrowed = borrowRecords.Any()
                ? borrowRecords.Max(b => b.BorrowDate)
                : null;

            return new BookDetailsDto
            {
                Id = book.Id,
                Title = book.Title,
                Version = book.Version,
                PublishDate = book.PublishDate,
                AuthorName = book.Author.AuthorName,
                CategoryName = book.Category.Name,
                PublisherName = book.Publisher.Name,
                TotalCopies = copies.Count(),
                AvailableCopies = available,
                BorrowedCopies = borrowed,
                LastBorrowedDate = lastBorrowed,
                IsDeleted = book.IsDeleted
            };
        }

        public async Task EditBook(int id, BookUpdateDto dto)
        {
            var book = await _bookRepo.GetByIdAsync(id);
            if (book == null || book.IsDeleted)
                throw new Exception("Book not found");

            book.Title = dto.Title;
            book.PublishDate = dto.PublishDate;
            book.Version = dto.Version;
            book.AuthorId = dto.AuthorId;
            book.CategoryId = dto.CategoryId;
            book.PublisherId = dto.PublisherId;
            book.LastModifiedBy = 1;
            book.LastModifiedDate = DateTime.Now;

            await _bookRepo.Update(book);
            await _bookRepo.SaveAsync();
        }
        public async Task DeleteBook(int id)
        {
            var book = await _bookRepo.GetByIdAsync(id);
            if (book == null)
                throw new Exception("Book not found");

            book.IsDeleted = true;
            book.DeletedBy = 1;
            book.DeletedDate = DateTime.Now;

            await _bookRepo.Update(book);
            await _bookRepo.SaveAsync();
        }
    }
}
