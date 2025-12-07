using LibrarySystem.Service;
using LibrarySystem.Shared.DTOs;
using LibrarySystem.Shared.DTOs.AuthorDtos;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly AuthorService _service;

        public AuthorController(AuthorService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AuthorCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.AuthorName))
                return BadRequest("AuthorName is required");

            await _service.AddAuthor(dto);
            return Ok("Author added successfully");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAuthors());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(await _service.GetAuthorById(id));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] AuthorUpdateDto dto)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");

            try
            {
                await _service.EditAuthor(id, dto);
                return Ok("Author updated successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAuthor(id);
                return Ok("Author deleted successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
