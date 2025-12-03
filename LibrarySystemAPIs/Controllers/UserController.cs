using LibrarySystem.DTOs.UserDtos;
using LibrarySystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemAPIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserCreateDto dto)
        {
            await _service.AddUser(dto);
            return Ok("User added successfully");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.ListUsers());
        }

        [HttpPut("Update/{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] UserUpdateDto dto)
        {
            await _service.EditUser(id, dto);
            return Ok("User updated successfully");
        }

        [HttpPut("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteUser(id);
            return Ok("User deleted successfully");
        }
    }
}
