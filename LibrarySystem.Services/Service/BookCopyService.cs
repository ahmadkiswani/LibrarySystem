using LibrarySystem.Domain.Repositories;
using LibrarySystem.Entities.Models;
using LibrarySystem.Services.Interfaces;
using LibrarySystem.Shared.DTOs.AvailableBookDto;
using LibrarySystem.Shared.DTOs.BookDtos;
using Microsoft.EntityFrameworkCore;

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
            using var transaction = await _copyRepo.Context.Database.BeginTransactionAsync();

            try
            {
                var book = await _bookRepo.GetByIdAsync(dto.BookId);
                if (book == null)
                    throw new Exception("Book does not exist");

                int totalCopies = await _copyRepo.GetQueryable()
                    .CountAsync(c => c.BookId == dto.BookId);

                if (totalCopies >= 100)
                    throw new Exception("Maximum number of copies reached");

                var copy = new BookCopy
                {
                    BookId = dto.BookId,
                    AuthorId = book.AuthorId,
                    CategoryId = book.CategoryId,
                    PublisherId = book.PublisherId,
                    CopyCode = Guid.NewGuid().ToString()[..8],
                    IsAvailable = true  

                };

                await _copyRepo.AddAsync(copy);
                book.TotalCopies += 1;
                await _bookRepo.UpdateAsync(book);

                await _copyRepo.SaveAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
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
            var copies = await _copyRepo.GetAllAsync();

            return copies
                .Select(c => new BookCopyListDto
                {
                    Id = c.Id,
                    BookId = c.BookId,
                    IsAvailable = c.IsAvailable
                })
                .ToList();
        }

        public async Task<BookCopy> GetSpecificCopy(int id)
        {
            var copy = await _copyRepo.GetByIdAsync(id);

            if (copy == null)
                throw new Exception("Copy not found");

            return copy;
        }

        public async Task<int> GetAllCopiesCount(int bookId)
        {
            return await _copyRepo.GetQueryable()
                .Where(c => c.BookId == bookId)
                .CountAsync();
        }

        public async Task<int> GetAvailableCount(int bookId)
        {
            return await _copyRepo.GetQueryable()
                .Where(c => c.BookId == bookId && c.IsAvailable)
                .CountAsync();
        }

        public async Task<int> GetBorrowedCount(int bookId)
        {
            return await _copyRepo.GetQueryable()
                .Where(c => c.BookId == bookId && !c.IsAvailable)
                .CountAsync();
        }
    }
}
