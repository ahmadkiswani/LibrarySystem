using LibrarySystem.Services.Interfaces;
using LibrarySystem.Shared.DTOs.BorrowDTOs;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _service;

        public BorrowController(IBorrowService service)
        {
            _service = service;
        }

        [HttpPost("Take")]
        public async Task<IActionResult> BorrowBook([FromBody] BorrowCreateDto dto)
        {
            try
            {
                await _service.BorrowBook(dto);
                return Ok("Book borrowed successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Return")]
        public async Task<IActionResult> ReturnBook([FromBody] BorrowReturnDto dto)
        {
            try
            {
                await _service.ReturnBook(dto);
                return Ok("Book returned successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
