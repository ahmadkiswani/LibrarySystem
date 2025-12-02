using LibrarySystem.DTOs;
using LibrarySystem.DTOs.AuthorDtos;
using LibrarySystem.DTOs.BookDtos;
using LibrarySystem.Models;
using Microsoft.EntityFrameworkCore;
using LibrarySystem.Data;
namespace LibrarySystem.Service
{
    public class AuthorService
    {
        private readonly LibraryDbContext _context;

        public AuthorService(LibraryDbContext context)
        {
            _context = context;
        }

        public void AddAuthor(AuthorCreateDto dto)
        {
            var author = new Author
            {
                AuthorName = dto.AuthorName,
                CreatedBy = 1,
                CreatedDate = DateTime.Now
            };

            _context.Authors.Add(author);
            _context.SaveChanges();
        }

        public List<AuthorListDto> GetAllAuthors()
        {
            return _context.Authors
                .Where(a => !a.IsDeleted)
                .Select(a => new AuthorListDto
                {
                    Id = a.Id,
                    AuthorName = a.AuthorName
                })
                .ToList();
        }

        public AuthorDetailsDto GetAuthorById(int id)
        {
            var author = _context.Authors
                .Include(a => a.Books)
                .FirstOrDefault(a => a.Id == id && !a.IsDeleted);

            if (author == null)
                throw new Exception("Author not found");

            return new AuthorDetailsDto
            {
                Id = author.Id,
                AuthorName = author.AuthorName,
                BooksCount = author.Books.Count(b => !b.IsDeleted)
            };
        }

        public void EditAuthor(int id, AuthorUpdateDto dto)
        {
            var author = _context.Authors.FirstOrDefault(x => x.Id == id && !x.IsDeleted);

            if (author == null)
                throw new Exception("Author not found");

            author.AuthorName = dto.AuthorName;
            author.LastModifiedBy = dto.Id;
            author.LastModifiedDate = DateTime.Now;

            _context.SaveChanges();
        }

        public void DeleteAuthor(int id)
        {
            var author = _context.Authors.FirstOrDefault(x => x.Id == id);

            if (author == null)
                throw new Exception("Author not found");

            author.IsDeleted = true;
            author.DeletedBy = 1;
            author.DeletedDate = DateTime.Now;

            _context.SaveChanges();
        }

        public List<AuthorListDto> Search(AuthorSearchDto dto)
        {
            int page = dto.Page <= 0 ? 1 : dto.Page;
            int pageSize = dto.PageSize <= 0 || dto.PageSize > 200 ? 10 : dto.PageSize;

            return _context.Authors
                .Where(a => !a.IsDeleted)
                .Where(a =>
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
        }

        public List<BookListDto> GetBooksByAuthor(int authorId)
        {
            return _context.Books
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
