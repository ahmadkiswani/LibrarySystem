using LibrarySystem.DTOs;
using LibrarySystem.DTOs.AuthorDtos;
using LibrarySystem.Models;

public class AuthorService
{
    private List<Author> _authors;
    private int _idCounter = 1;

    public AuthorService(List<Author> authors)
    {
        _authors = authors;
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
        var author = _authors.FirstOrDefault(x => x.Id == id);

        if (author == null)
            return null;

        AuthorDetailsDto dto = new AuthorDetailsDto();
        dto.Id = author.Id;
        dto.AuthorName = author.AuthorName;
        dto.BooksCount = author.Books.Count;

        return dto;
    }
    public void EditAuthor(int id, AuthorUpdateDto dto)
    {
        var author = _authors.FirstOrDefault(x => x.Id == id);

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
        {
            _authors.Remove(author);
        }
    }
}