using Microsoft.AspNetCore.Mvc;
using LibrarySystem.Service;
using LibrarySystem.DTOs.AvailableBookDto;

namespace LibrarySystemAPIs.Controllers
{
    [ApiController]
    [Route("api/BookCopy")]
    public class BookCopyController : ControllerBase
    {
        private readonly BookCopyService _service;

        public BookCopyController(BookCopyService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Add([FromBody] BookCopyCreateDto dto)
        {
            if (dto.BookId <= 0)
                return BadRequest("Invalid Book ID");

            try
            {
                _service.AddBookCopy(dto);
                return Ok("Book copy added successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");

            try
            {
                _service.DeleteBookCopy(id);
                return Ok("Book copy deleted successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.ListBookCopies());
        }
    }
}
