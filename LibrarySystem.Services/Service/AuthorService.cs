using LibrarySystem.Domain.Repositories;
using LibrarySystem.Entities.Models;
using LibrarySystem.Services.Interfaces;
using LibrarySystem.Shared.DTOs;
using LibrarySystem.Shared.DTOs.AuthorDtos;
using LibrarySystem.Shared.DTOs.BookDtos;

namespace LibrarySystem.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IGenericRepository<Author> _authorRepo;
        private readonly IGenericRepository<Book> _bookRepo;

        public AuthorService(
            IGenericRepository<Author> authorRepo,
            IGenericRepository<Book> bookRepo)
        {
            _authorRepo = authorRepo;
            _bookRepo = bookRepo;
        }

        public async Task AddAuthor(AuthorCreateDto dto)
        {
            var author = new Author
            {
                AuthorName = dto.AuthorName
            };

            await _authorRepo.AddAsync(author);
            await _authorRepo.SaveAsync();
        }

        public async Task<List<AuthorListDto>> GetAllAuthors()
        {
            var authors = await _authorRepo.FindAsync(a => !a.IsDeleted);

            return authors
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

            var books = await _bookRepo.FindAsync(b => b.AuthorId == id && !b.IsDeleted);

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

            await _authorRepo.UpdateAsync(author);
            await _authorRepo.SaveAsync();
        }

        public async Task DeleteAuthor(int id)
        {
            var author = await _authorRepo.GetByIdAsync(id);

            if (author == null)
                throw new Exception("Author not found");

            await _authorRepo.SoftDeleteAsync(author);
            await _authorRepo.SaveAsync();
        }

        public async Task<List<AuthorListDto>> Search(AuthorSearchDto dto)
        {
            int page = dto.Page <= 0 ? 1 : dto.Page;
            int pageSize = dto.PageSize <= 0 || dto.PageSize > 200 ? 10 : dto.PageSize;

            var authors = await _authorRepo.FindAsync(a =>
                !a.IsDeleted &&
                (dto.Text == null || a.AuthorName.ToLower().Contains(dto.Text.ToLower())) &&
                (!dto.Number.HasValue || a.Id == dto.Number.Value)
            );

            return authors
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AuthorListDto
                {
                    Id = a.Id,
                    AuthorName = a.AuthorName
                })
                .ToList();
        }

        public async Task<List<BookListDto>> GetBooksByAuthor(int authorId)
        {
            var books = await _bookRepo.FindAsync(b => b.AuthorId == authorId && !b.IsDeleted);

            return books
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
