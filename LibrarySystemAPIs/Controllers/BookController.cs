using Microsoft.AspNetCore.Mvc;
using LibrarySystem.Services.Interfaces;
using LibrarySystem.Shared.DTOs.BookDtos;

namespace LibrarySystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _service;

        public BookController(IBookService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] BookCreateDto dto)
        {
            var id = await _service.AddBook(dto);
            return Ok(id);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllBooks());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _service.GetBookById(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] BookUpdateDto dto)
        {
            await _service.EditBook(id, dto);
            return Ok("Book updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteBook(id);
            return Ok("Book deleted successfully");
        }
    }
}
