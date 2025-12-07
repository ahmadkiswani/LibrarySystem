using LibrarySystem.Services.Interfaces;
using LibrarySystem.Shared.DTOs.UserDtos;
using Microsoft.AspNetCore.Mvc;

    namespace LibrarySystemAPIs.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class UserController : ControllerBase
        {
            private readonly IUserService _service;

            public UserController(IUserService service)
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
            public async Task<IActionResult> Delete(int id, [FromBody] UserDeleteDto dto)
            {
                await _service.DeleteUser(id, dto);
                return Ok("User deleted successfully");
            }
        }
    }
