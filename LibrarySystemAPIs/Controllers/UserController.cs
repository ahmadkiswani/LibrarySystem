using Microsoft.AspNetCore.Mvc;
using LibrarySystem.Service;
using LibrarySystem.DTOs.UserDtos;

namespace LibrarySystemAPIs.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }
        [HttpPost]
        public IActionResult Add([FromBody] UserCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserName))
                return BadRequest("Username is required");  
            if (string.IsNullOrWhiteSpace(dto.UserEmail))
                return BadRequest("Email is required");
            _service.AddUser(dto);
            return Ok("User added successfully");
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_service.ListUsers());
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = _service.GetUserById(id);
            if (result == null)
                return NotFound("User not found");
            return Ok(result);
        }
        [HttpPut("{id}")]
        public IActionResult Edit(int id, [FromBody] UserUpdateDto dto)
        {
            if (id <= 0)
                return BadRequest("Invalid ID");
            _service.EditUser(id, dto);
            return Ok("User updated successfully");
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _service.DeleteUser(id);
                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}