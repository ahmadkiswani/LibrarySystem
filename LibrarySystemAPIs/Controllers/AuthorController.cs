using LibrarySystem.DTOs;
using LibrarySystem.DTOs.AuthorDtos;
using LibrarySystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemAPIs.Controllers
{
    [ApiController]
    [Route("api/Author")]
    public class AuthorController : ControllerBase
    {
        private readonly AuthorService _service;

        public AuthorController(AuthorService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Add([FromBody] AuthorCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.AuthorName))
                return BadRequest("AuthorName is required");

            _service.AddAuthor(dto);
            return Ok("Author added successfully");
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.GetAllAuthors());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _service.GetAuthorById(id);

            try
            {
                return Ok(_service.GetAuthorById(id));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }

        [HttpPut("Update/{id}")]
        public IActionResult Edit(int id, [FromBody] AuthorUpdateDto dto)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");

            _service.EditAuthor(id, dto);
            return Ok("Author updated successfully");
        }

        [HttpPut("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _service.DeleteAuthor(id);
                return Ok("Author deleted successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
