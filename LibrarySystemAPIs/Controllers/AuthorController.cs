using LibrarySystem.Services.Interfaces;
using LibrarySystem.Shared.DTOs;
using LibrarySystem.Shared.DTOs.AuthorDtos;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _service;

        public AuthorController(IAuthorService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddAuthor([FromBody] AuthorCreateDto dto)
        {
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
            return Ok(await _service.GetAuthorById(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] AuthorUpdateDto dto)
        {
            await _service.EditAuthor(id, dto);
            return Ok("Author updated successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAuthor(id);
            return Ok("Author deleted successfully");
        }
    }
}
