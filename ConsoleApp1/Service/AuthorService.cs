using LibrarySystem.DTOs;
using LibrarySystem.DTOs.AuthorDtos;
using LibrarySystem.DTOs.BookDtos;
using LibrarySystem.Models;
using static System.Reflection.Metadata.BlobBuilder;

public class AuthorService
{
    private List<Author> _authors;
    private readonly List<Book> _books;

    private int _idCounter = 1;

    public AuthorService(List<Author> authors, List<Book> books )
    {
        _authors = authors;
        _books = books;
    }


    public void AddAuthor(AuthorCreateDto dto)
    {
        var author = new Author();
        author.Id = _idCounter++;
        author.AuthorName = dto.AuthorName;
        author.CreatedBy=1;
        author.CreatedDate = DateTime.Now;
        _authors.Add(author);
    }

   
    public List<AuthorListDto> GetAllAuthors()
    {
        List<AuthorListDto> list = new List<AuthorListDto>();
        return _authors
               .Select(a => new AuthorListDto
               {
                   Id = a.Id,
                   AuthorName = a.AuthorName
               }).ToList();
    }

 
    public AuthorDetailsDto GetAuthorById(int id)
    {
        var author = _authors.FirstOrDefault(x => x.Id == id && !x.IsDeleted);

        if (author == null)
            return null;

        AuthorDetailsDto dto = new AuthorDetailsDto();
        dto.Id = author.Id;
        dto.AuthorName = author.AuthorName;
        dto.BooksCount = author.Books.Count(b => !b.IsDeleted);

        return dto;
    }
    public void EditAuthor(int id, AuthorUpdateDto dto)
    {
        var author = _authors.FirstOrDefault(x => x.Id == id && !x.IsDeleted);

        if (author != null)
        {
            author.AuthorName = dto.AuthorName;
            author.LastModifiedBy =dto.Id;
            author.LastModifiedDate = DateTime.Now;
        }
    }
    public void DeleteAuthor(int id)
    {
        var author = _authors.FirstOrDefault(x => x.Id == id);

        if (author != null)
            throw new Exception("Author not found");
        author.IsDeleted = true;
        author.DeletedBy = 0;
        author.DeletedDate = DateTime.Now;
    }
    public List<BookListDto> GetBooksByAuthor(int authorId)
    {
        return _books
            .Where(b => b.AuthorId == authorId)
            .Select(b => new BookListDto
            {
                Id = b.Id,
                Title = b.Title,
                AvailableCopies = b.TotalCopies
            })
            .ToList();
    }
}