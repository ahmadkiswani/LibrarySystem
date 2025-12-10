        using LibrarySystem.Domain.Repositories;
    using LibrarySystem.Entities.Models;
    using LibrarySystem.Services.Interfaces;
    using LibrarySystem.Shared.DTOs.BookDtos;
    using Microsoft.EntityFrameworkCore;

    namespace LibrarySystem.Services
    {

        public class BookService : IBookService
        {
            private readonly IGenericRepository<Book> _bookRepo;
            private readonly IGenericRepository<BookCopy> _copyRepo;
            private readonly IGenericRepository<Borrow> _borrowRepo;
            private readonly IGenericRepository<Author> _authorRepo;
            private readonly IGenericRepository<Category> _categoryRepo;
            private readonly IGenericRepository<Publisher> _publisherRepo;

        public BookService(
                 IGenericRepository<Book> bookRepo,
                 IGenericRepository<BookCopy> copyRepo,
                 IGenericRepository<Borrow> borrowRepo,
                 IGenericRepository<Author> authorRepo,
                 IGenericRepository<Category> categoryRepo,
                 IGenericRepository<Publisher> publisherRepo)
        {
                _bookRepo = bookRepo;
                _copyRepo = copyRepo;
                _borrowRepo = borrowRepo;
                _authorRepo = authorRepo;
                _categoryRepo = categoryRepo;
                _publisherRepo = publisherRepo;
        }


        public async Task<int> AddBook(BookCreateDto dto)
        {
            var authorExists = await _authorRepo.GetQueryable()
                .AnyAsync(a => a.Id == dto.AuthorId);
            if (!authorExists)
                throw new Exception("Author does not exist");

            var categoryExists = await _categoryRepo.GetQueryable()
                .AnyAsync(c => c.Id == dto.CategoryId);
            if (!categoryExists)
                throw new Exception("Category does not exist");

            var publisherExists = await _publisherRepo.GetQueryable()
                .AnyAsync(p => p.Id == dto.PublisherId);
            if (!publisherExists)
                throw new Exception("Publisher does not exist");

            bool bookExists = await _bookRepo.GetQueryable()
                .AnyAsync(b => b.Title == dto.Title
                               && b.Version == dto.Version
                               && b.AuthorId == dto.AuthorId);

            if (bookExists)
                throw new Exception("Book already exists for this author.");

            var book = new Book
            {
                Title = dto.Title,
                Version = dto.Version,
                AuthorId = dto.AuthorId,
                CategoryId = dto.CategoryId,
                PublisherId = dto.PublisherId
            };

            await _bookRepo.AddAsync(book);
            await _bookRepo.SaveAsync();

            return book.Id;
        }

        public async Task<List<BookListDto>> GetAllBooks()
        {
            var books = await _bookRepo.GetAllAsync();

            return books.Select(b => new BookListDto
            {
                Id = b.Id,
                Title = b.Title,
                TotalCopies = b.TotalCopies
            }).ToList();
        }


        public async Task<BookDetailsDto> GetBookById(int id)
            {
                var book = await _bookRepo.GetQueryable()
                    .Include(b => b.Author)
                    .Include(b => b.Category)
                    .Include(b => b.Publisher)
                    .FirstOrDefaultAsync(b => b.Id == id);

                if (book == null)
                    throw new Exception("Book not found");

            
                var copies = await _copyRepo.GetQueryable()
                    .Where(c => c.BookId == id)
                    .ToListAsync();

                int available = copies.Count(c => c.IsAvailable);
                int borrowed = copies.Count(c => !c.IsAvailable);

                var borrowRecords = await _borrowRepo.GetQueryable()
                    .Include(b => b.BookCopy)
                    .Where(b => b.BookCopy.BookId == id)
                    .ToListAsync();

                DateTime? lastBorrowed = borrowRecords.Any()
                    ? borrowRecords.Max(b => b.BorrowDate)
                    : null;

                return new BookDetailsDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Version = book.Version,
                    PublishDate = book.PublishDate,
                    AuthorName = book.Author?.AuthorName,
                    CategoryName = book.Category?.Name,
                    PublisherName = book.Publisher?.Name,
                    TotalCopies = copies.Count,
                    AvailableCopies = available,
                    BorrowedCopies = borrowed,
                    LastBorrowedDate = lastBorrowed,
                    IsDeleted = book.IsDeleted
                };
            }

            public async Task EditBook(int id, BookUpdateDto dto)
            {
                var book = await _bookRepo.GetByIdAsync(id);
                if (book == null)
                    throw new Exception("Book not found");

                book.Title = dto.Title;
                book.PublishDate = dto.PublishDate;
                book.Version = dto.Version;
                book.AuthorId = dto.AuthorId;
                book.CategoryId = dto.CategoryId;
                book.PublisherId = dto.PublisherId;

                await _bookRepo.UpdateAsync(book);
                await _bookRepo.SaveAsync();
            }

            public async Task DeleteBook(int id)
            {
                var book = await _bookRepo.GetByIdAsync(id);
                if (book == null)
                    throw new Exception("Book not found");

                await _bookRepo.SoftDeleteAsync(book);
                await _bookRepo.SaveAsync();
            }

        public async Task<List<BookListDto>> SearchBooks(BookSearchDto dto)
        {

            int page = dto.Page.HasValue && dto.Page.Value > 0 ? dto.Page.Value : 1;
            int pageSize = dto.PageSize.HasValue && dto.PageSize.Value > 0 ? dto.PageSize.Value : 10;

            int? author = dto.AuthorId > 0 ? dto.AuthorId : null;
            int? category = dto.CategoryId > 0 ? dto.CategoryId : null;
            int? publisher = dto.PublisherId > 0 ? dto.PublisherId : null;

            var query = _bookRepo.GetQueryable()
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Include(b => b.Publisher)
                .Where(b =>
                    (string.IsNullOrWhiteSpace(dto.Title) ||
                     b.Title.ToLower().Contains(dto.Title.ToLower()))

                    && (author == null || b.AuthorId == author)
                    && (category == null || b.CategoryId == category)
                    && (publisher == null || b.PublisherId == publisher)
                );

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BookListDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    AuthorName = b.Author.AuthorName,
                    CategoryName = b.Category.Name,
                    PublisherName = b.Publisher.Name,
                    TotalCopies = _copyRepo.GetQueryable().Count(c => c.BookId == b.Id),
                    AvailableCopies = _copyRepo.GetQueryable().Count(c => c.BookId == b.Id && c.IsAvailable)
                })
                .ToListAsync();
        }




        public async Task<BookDetailsDto> GetBookDetails(int id)
        {
            var book = await _bookRepo.GetByIdAsync(id);
            if (book == null)
                throw new Exception("Book not found");

            int totalCopies = await _copyRepo.GetQueryable()
                .CountAsync(c => c.BookId == id);

            int availableCopies = await _copyRepo.GetQueryable()
                .CountAsync(c => c.BookId == id && c.IsAvailable);

            int borrowedCopies = totalCopies - availableCopies;

            return new BookDetailsDto
            {
                Id = book.Id,
                Title = book.Title,
                Version = book.Version,
                PublishDate = book.PublishDate,

                AuthorName = "Unknown",   
                CategoryName = "Unknown",  
                PublisherName = "Unknown",

                TotalCopies = totalCopies,
                AvailableCopies = availableCopies,
                BorrowedCopies = borrowedCopies,

                LastBorrowedDate = null,
                IsDeleted = book.IsDeleted
            };
        }






    }
}
