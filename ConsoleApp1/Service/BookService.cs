using LibrarySystem.DTOs.BookDtos;
using LibrarySystem.Models;
using LibrarySystem.Repository;

namespace LibrarySystem.Service
{
    public class BookService
    {
        private readonly IGenericRepository<Book> _bookRepo;

        public BookService(IGenericRepository<Book> bookRepo)
        {
            _bookRepo = bookRepo;
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
            var book = await _bookRepo.GetByIdAsync(id);
            if (book == null || book.IsDeleted)
                throw new Exception("Book not found");

            return new BookDetailsDto
            {
                Id = book.Id,
                Title = book.Title,
                PublishDate = book.PublishDate,
                Version = book.Version,
                AuthorId = book.AuthorId,
                CategoryId = book.CategoryId,
                TotalCopies = book.TotalCopies
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
