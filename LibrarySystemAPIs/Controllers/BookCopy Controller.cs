using LibrarySystem.DTOs.AvailableBookDto;
using LibrarySystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookCopyController : ControllerBase
    {
        private readonly BookCopyService _service;

        public BookCopyController(BookCopyService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BookCopyCreateDto dto)
        {
            if (dto.BookId <= 0)
                return BadRequest("Invalid Book ID");

            await _service.AddBookCopy(dto);
            return Ok("Book copy added successfully");
        }

        [HttpPut("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");

            try
            {
                await _service.DeleteBookCopy(id);
                return Ok("Book copy deleted successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.ListBookCopies());
        }

        [HttpGet("copy/{id}")]
        public async Task<IActionResult> GetSpecific(int id)
        {
            try
            {
                return Ok(await _service.GetSpecificCopy(id));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("total/{bookId}")]
        public async Task<IActionResult> GetTotal(int bookId)
        {
            return Ok(await _service.GetAllCopiesCount(bookId));
        }

        [HttpGet("available/{bookId}")]
        public async Task<IActionResult> GetAvailable(int bookId)
        {
            return Ok(await _service.GetAvailableCount(bookId));
        }

        [HttpGet("borrowed/{bookId}")]
        public async Task<IActionResult> GetBorrowed(int bookId)
        {
            return Ok(await _service.GetBorrowedCount(bookId));
        }
    }
}
