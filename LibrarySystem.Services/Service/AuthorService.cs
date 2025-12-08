using LibrarySystem.Domain.Repositories;
using LibrarySystem.Entities.Models;
using LibrarySystem.Services.Interfaces;
using LibrarySystem.Shared.DTOs.AuthorDtos;
using LibrarySystem.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IGenericRepository<Author> _authorRepo;

        public AuthorService(IGenericRepository<Author> authorRepo)
        {
            _authorRepo = authorRepo;
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
            var authors = await _authorRepo.GetAllAsync();

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

            if (author == null)
                throw new Exception("Author not found");

            return new AuthorDetailsDto
            {
                Id = author.Id,
                AuthorName = author.AuthorName
            };
        }

        public async Task EditAuthor(int id, AuthorUpdateDto dto)
        {
            var author = await _authorRepo.GetByIdAsync(id);
            if (author == null)
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
            var query = _authorRepo.Query();

            if (!string.IsNullOrWhiteSpace(dto.Text))
                query = query.Where(a => a.AuthorName.Contains(dto.Text));

            if (dto.Number.HasValue)
                query = query.Where(a => a.Id == dto.Number.Value);

            int page = dto.Page <= 0 ? 1 : dto.Page;
            int pageSize = dto.PageSize <= 0 || dto.PageSize > 200 ? 10 : dto.PageSize;

            return await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AuthorListDto
                {
                    Id = a.Id,
                    AuthorName = a.AuthorName
                })
                .ToListAsync();
        }
    }
}
