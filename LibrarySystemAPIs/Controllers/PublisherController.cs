
using LibrarySystem.Service;
using LibrarySystem.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublisherController : ControllerBase
    {
        private readonly PublisherService _service;

        public PublisherController(PublisherService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PublisherCreateDto dto)
        {
            await _service.AddPublisher(dto);
            return Ok("Publisher added successfully");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.ListPublishers());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, [FromBody] PublisherDetailsDto dto)
        {
            try
            {
                return Ok(await _service.GetAllPublishers());
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] PublisherUpdateDto dto)
        {
            await _service.EditPublisher(id, dto);
            return Ok("Publisher updated successfully");
        }

        [HttpPut("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeletePublisher(id);
            return Ok("Publisher deleted successfully");
        }
    }
}
