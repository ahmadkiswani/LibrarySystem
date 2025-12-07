using LibrarySystem.Domain.Repositories;
using LibrarySystem.Entities.Models;
using LibrarySystem.Services.Interfaces;
using LibrarySystem.Shared.DTOs.AvailableBookDto;
using LibrarySystem.Shared.DTOs.BookDtos;

namespace LibrarySystem.Services
{
    public class BookCopyService : IBookCopyService
    {
        private readonly IGenericRepository<BookCopy> _copyRepo;
        private readonly IGenericRepository<Book> _bookRepo;

        public BookCopyService(
            IGenericRepository<BookCopy> copyRepo,
            IGenericRepository<Book> bookRepo)
        {
            _copyRepo = copyRepo;
            _bookRepo = bookRepo;
        }

        public async Task AddBookCopy(BookCopyCreateDto dto)
        {
            var book = await _bookRepo.GetByIdAsync(dto.BookId);

            if (book == null || book.IsDeleted)
                throw new Exception("Book not found");

            var copy = new BookCopy
            {
                BookId = dto.BookId,
                AuthorId = book.AuthorId,
                CategoryId = book.CategoryId,
                PublisherId = book.PublisherId,
                CopyCode = Guid.NewGuid().ToString().Substring(0, 8)
            };

            await _copyRepo.AddAsync(copy);

            book.TotalCopies += 1;
            await _bookRepo.UpdateAsync(book);

            await _copyRepo.SaveAsync();
        }

        public async Task DeleteBookCopy(int id)
        {
            var copy = await _copyRepo.GetByIdAsync(id);

            if (copy == null)
                throw new Exception("Copy not found");

            await _copyRepo.SoftDeleteAsync(copy);
            await _copyRepo.SaveAsync();
        }

        public async Task<List<BookCopyListDto>> ListBookCopies()
        {
            var copies = await _copyRepo.FindAsync(c => !c.IsDeleted);

            return copies.Select(c => new BookCopyListDto
            {
                Id = c.Id,
                BookId = c.BookId,
                IsAvailable = c.IsAvailable
            }).ToList();
        }

        public async Task<BookCopy> GetSpecificCopy(int id)
        {
            var copy = await _copyRepo.GetByIdAsync(id);
            if (copy == null || copy.IsDeleted)
                throw new Exception("Copy not found");

            return copy;
        }

        public async Task<int> GetAllCopiesCount(int bookId)
        {
            var copies = await _copyRepo.FindAsync(c => c.BookId == bookId && !c.IsDeleted);
            return copies.Count();
        }

        public async Task<int> GetAvailableCount(int bookId)
        {
            var copies = await _copyRepo.FindAsync(c => c.BookId == bookId && !c.IsDeleted && c.IsAvailable);
            return copies.Count();
        }

        public async Task<int> GetBorrowedCount(int bookId)
        {
            var copies = await _copyRepo.FindAsync(c => c.BookId == bookId && !c.IsDeleted && !c.IsAvailable);
            return copies.Count();
        }
    }
}
