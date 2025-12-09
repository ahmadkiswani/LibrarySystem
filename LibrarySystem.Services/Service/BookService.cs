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
        public async Task<List<BookListDto>> GetBooksByAuthor(int authorId)
        {
            return await _bookRepo.GetQueryable()
                .Where(b => b.AuthorId == authorId)
                .Select(b => new BookListDto
                {
                    Id = b.Id,
                    Title = b.Title
                })
                .ToListAsync();
        }

        public async Task<List<BookListDto>> GetBooksByCategory(int categoryId)
        {
            return await _bookRepo.GetQueryable()
                .Where(b => b.CategoryId == categoryId)
                .Select(b => new BookListDto
                {
                    Id = b.Id,
                    Title = b.Title
                })
                .ToListAsync();
        }

        public async Task<List<BookListDto>> GetBooksByPublisher(int publisherId)
        {
            return await _bookRepo.GetQueryable()
                .Where(b => b.PublisherId == publisherId)
                .Select(b => new BookListDto
                {
                    Id = b.Id,
                    Title = b.Title
                })
                .ToListAsync();
        }
        public async Task<List<BookListDto>> SearchBooks(BookSearchDto dto)
        {
            var query =
                _bookRepo.GetQueryable()

                .Where(b =>
                    (string.IsNullOrEmpty(dto.Text) ||
                        b.Title.ToLower().Contains(dto.Text.ToLower()))
                    &&
                    (string.IsNullOrEmpty(dto.Title) ||
                        b.Title.ToLower().Contains(dto.Title.ToLower()))
                )

               
                .Where(b =>
                    string.IsNullOrEmpty(dto.Version) ||
                    b.Version == dto.Version
                )

                .Where(b =>
                    !dto.PublishDate.HasValue ||
                    b.PublishDate == dto.PublishDate.Value
                )

                .Where(b =>
                    (!dto.AuthorId.HasValue || b.AuthorId == dto.AuthorId.Value) &&
                    (!dto.CategoryId.HasValue || b.CategoryId == dto.CategoryId.Value) &&
                    (!dto.PublisherId.HasValue || b.PublisherId == dto.PublisherId.Value)
                )

                .Where(b =>
                    !dto.Available.HasValue ||
                    (
                        dto.Available.Value
                            ? _copyRepo.GetQueryable().Any(c => c.BookId == b.Id && c.IsAvailable)
                            : !_copyRepo.GetQueryable().Any(c => c.BookId == b.Id && c.IsAvailable)
                    )
                );

            query = dto.SortBy?.ToLower() switch
            {
                "title" => dto.SortDescending ? query.OrderByDescending(b => b.Title) : query.OrderBy(b => b.Title),
                "publishdate" => dto.SortDescending ? query.OrderByDescending(b => b.PublishDate) : query.OrderBy(b => b.PublishDate),
                "version" => dto.SortDescending ? query.OrderByDescending(b => b.Version) : query.OrderBy(b => b.Version),
                _ => query.OrderBy(b => b.Title) 
            };

            int page = dto.Page > 0 ? dto.Page : 1;
            int pageSize = dto.PageSize > 0 ? dto.PageSize : 10;

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BookListDto
                {
                    Id = b.Id,
                    Title = b.Title
                })
                .ToListAsync();
        }

       


    }
}
