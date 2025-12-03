using LibrarySystem.DTOs.BookDtos;
using LibrarySystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly BookService _service;

        public BookController(BookService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BookCreateDto dto)
        {
            var newBookId = await _service.AddBook(dto);
            return Ok(newBookId);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllBooks());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(await _service.GetBookById(id));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] BookUpdateDto dto)
        {
            await _service.EditBook(id, dto);
            return Ok("Book updated successfully");
        }

        [HttpPut("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteBook(id);
            return Ok("Book deleted successfully");
        }
    }
}
