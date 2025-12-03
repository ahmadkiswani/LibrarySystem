using LibrarySystem.DTOs;
using LibrarySystem.DTOs.AuthorDtos;
using LibrarySystem.DTOs.BookDtos;
using LibrarySystem.Models;
using LibrarySystem.Repository;

namespace LibrarySystem.Service
{
    public class AuthorService
    {
        private readonly IGenericRepository<Author> _authorRepo;
        private readonly IGenericRepository<Book> _bookRepo;

        public AuthorService(IGenericRepository<Author> authorRepo,
            IGenericRepository<Book> bookRepo)
        {
            _authorRepo = authorRepo;
            _bookRepo = bookRepo;
        }

        public async Task AddAuthor(AuthorCreateDto dto)
        {
            var author = new Author
            {
                AuthorName = dto.AuthorName,
                CreatedBy = 0,
                CreatedDate = DateTime.Now
            };

            await _authorRepo.AddAsync(author);
            await _authorRepo.SaveAsync();
        }

        public async Task<List<AuthorListDto>> GetAllAuthors()
        {
            var authors = await _authorRepo.GetAllAsync();

            return authors
                .Where(a => !a.IsDeleted)
                .Select(a => new AuthorListDto
                {
                    Id = a.Id,
                    AuthorName = a.AuthorName
                })
                .ToList();
        }

        public async Task<AuthorDetailsDto> GetAuthorById(int id)
        {
            var author = await _authorRepo.GetByIdAsync(id);

            if (author == null || author.IsDeleted)
                throw new Exception("Author not found");

            var books = (await _bookRepo.GetAllAsync())
                .Where(b => b.AuthorId == id && !b.IsDeleted)
                .ToList();

            return new AuthorDetailsDto
            {
                Id = author.Id,
                AuthorName = author.AuthorName,
                BooksCount = books.Count
            };
        }

        public async Task EditAuthor(int id, AuthorUpdateDto dto)
        {
            var author = await _authorRepo.GetByIdAsync(id);

            if (author == null || author.IsDeleted)
                throw new Exception("Author not found");

            author.AuthorName = dto.AuthorName;
            author.LastModifiedBy = dto.Id;
            author.LastModifiedDate = DateTime.Now;

            await _authorRepo.Update(author);
            await _authorRepo.SaveAsync();
        }

        public async Task DeleteAuthor(int id)
        {
            var author = await _authorRepo.GetByIdAsync(id);

            if (author == null)
                throw new Exception("Author not found");

            author.IsDeleted = true;
            author.DeletedBy = 1;
            author.DeletedDate = DateTime.Now;

            await _authorRepo.Update(author);
            await _authorRepo.SaveAsync();
        }

        public async Task<List<AuthorListDto>> Search(AuthorSearchDto dto)
        {
            int page = dto.Page <= 0 ? 1 : dto.Page;
            int pageSize = dto.PageSize <= 0 || dto.PageSize > 200 ? 10 : dto.PageSize;

            var authors = (await _authorRepo.GetAllAsync())
                .Where(a =>
                    !a.IsDeleted &&
                    (dto.Text == null || a.AuthorName.ToLower().Contains(dto.Text.ToLower())) &&
                    (dto.Number == null || a.Id == dto.Number)
                )
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AuthorListDto
                {
                    Id = a.Id,
                    AuthorName = a.AuthorName
                })
                .ToList();

            return authors;
        }

        public async Task<List<BookListDto>> GetBooksByAuthor(int authorId)
        {
            var books = await _bookRepo.GetAllAsync();

            return books
                .Where(b => b.AuthorId == authorId && !b.IsDeleted)
                .Select(b => new BookListDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    AvailableCopies = b.TotalCopies
                })
                .ToList();
        }
    }
}
