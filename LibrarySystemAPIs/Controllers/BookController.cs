using Microsoft.AspNetCore.Mvc;
using LibrarySystem.Service;
using LibrarySystem.DTOs.BookDtos;

namespace LibrarySystemAPIs.Controllers
{
    [ApiController]
    [Route("api/Book")]
    public class BookController : ControllerBase
    {
        private readonly BookService _service;

        public BookController(BookService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Add([FromBody] BookCreateDto dto)
        {
            if (dto == null)
                return BadRequest("Request body is empty");

            if (string.IsNullOrWhiteSpace(dto.Title))
                return BadRequest("Title is required");

            if (dto.AuthorId <= 0)
                return BadRequest("Invalid AuthorId");

            if (dto.CategoryId <= 0)
                return BadRequest("Invalid CategoryId");

            if (dto.PublisherId <= 0)
                return BadRequest("Invalid PublisherId");

            _service.AddBook(dto);
            return Ok("Book added successfully");
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAllBooks());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                return Ok(_service.GetBookById(id));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        
        }

        [HttpPut("Update/{id}")]
        public IActionResult Edit(int id, [FromBody] BookUpdateDto dto)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");

            _service.EditBook(id, dto);
            return Ok("Book updated successfully");
        }

        [HttpPut("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _service.DeleteBook(id);
                return Ok("Book deleted successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] BookSearchDto dto)
        {
            var result = _service.Search(dto);
            return Ok(result);
        }
    }
}
