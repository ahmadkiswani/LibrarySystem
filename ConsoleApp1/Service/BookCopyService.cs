using LibrarySystem.DTOs.AvailableBookDto;
using LibrarySystem.Models;
using LibrarySystem.Repository;

namespace LibrarySystem.Service
{
    public class BookCopyService
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
                IsAvailable = true,
                CreatedBy = 1,  
                CreatedDate = DateTime.Now
            };

            await _copyRepo.AddAsync(copy);
            await _copyRepo.SaveAsync();
        }



        public async Task DeleteBookCopy(int id)
        {
            var copy = await _copyRepo.GetByIdAsync(id);

            if (copy == null || copy.IsDeleted)
                throw new Exception("Copy not found");

            copy.IsDeleted = true;
            copy.IsAvailable = false;
            copy.LastModifiedBy = 1;
            copy.LastModifiedDate = DateTime.Now;

            await _copyRepo.Update(copy);
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
