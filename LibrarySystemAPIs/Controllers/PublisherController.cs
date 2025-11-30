using LibrarySystem.DTOs;
using LibrarySystem.Service;
using Microsoft.AspNetCore.Mvc;
namespace LibrarySystemAPIs.Controllers
{
    [ApiController]
    [Route("api/Publisher")]
    public class PublisherController : ControllerBase
    {
        private readonly PublisherService _service;

        public PublisherController(PublisherService service)
        {
            _service = service;
        }
        [HttpPost]
        public IActionResult Add([FromBody] PublisherCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest("Name is required");
            _service.AddPublisher(dto);
            return Ok("Publisher added successfully");
        }   
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.ListPublishers());
        }
        [HttpGet("{id}")]   
        public IActionResult GetById(int id)
        {
            var result = _service.GetPublisherById(id);
            if (result == null)
                return NotFound("Publisher not found");
            return Ok(result);
        }   
        [HttpPut("{id}")]
        public IActionResult Edit(int id, [FromBody] PublisherUpdateDto dto)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");
            _service.EditPublisher(id, dto);
            return Ok("Publisher updated successfully");
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _service.DeletePublisher(id);
                return Ok("Publisher deleted successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }


    }

}