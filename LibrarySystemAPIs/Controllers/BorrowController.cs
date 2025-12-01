using Microsoft.AspNetCore.Mvc;
using LibrarySystem.Service;
using LibrarySystem.DTOs.BorrowDTOs;

namespace LibrarySystemAPIs.Controllers
{
    [ApiController]
    [Route("api/Borrow")]
    public class BorrowController : ControllerBase
    {
        private readonly BorrowService _service;

        public BorrowController(BorrowService service)
        {
            _service = service;
        }

        [HttpPost("Take")]
        public IActionResult BorrowBook([FromBody] BorrowCreateDto dto)
        {
            if (dto.Id <= 0)
                return BadRequest("Invalid User ID");

            if (dto.BookCopyId <= 0)
                return BadRequest("Invalid BookCopy ID");

            try
            {
                _service.BorrowBook(dto);
                return Ok("Book borrowed successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Return")]
        public IActionResult ReturnBook([FromBody] BorrowReturnDto dto)
        {
            if (dto.Id <= 0)
                return BadRequest("Invalid borrow ID");
            try
            {
                _service.ReturnBook(dto);
                return Ok("Book returned successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("User/{userId}")]
        public IActionResult GetUserBorrowed(int userId)
        {
            try
            {
                var result = _service.GetBorrowedBooksByUser(userId);

                if (result == null || result.Count == 0)
                    return NotFound("User has no borrowed books");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
