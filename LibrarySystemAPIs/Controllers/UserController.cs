using LibrarySystem.API.Helpers;
using LibrarySystem.Services.Interfaces;
using LibrarySystem.Shared.DTOs.Helper;
using LibrarySystem.Shared.DTOs.HelperDto;
using LibrarySystem.Shared.DTOs.UserDtos;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var validation = ValidationHelper.ValidateDto( dto);
            if (!validation.IsValid)
            {
                return BadRequest(new BaseResponse<object>
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = validation.Errors
                });
            }

            try
            {
                await _service.AddUser(dto);

                return Ok(new BaseResponse<object>
                {
                    Success = true,
                    Message = "User added successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse<object>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _service.ListUsers();

            return Ok(new BaseResponse<object>
            {
                Success = true,
                Message = "Users retrieved successfully",
                Data = users
            });
        }
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetUserDetails(int id)
        {
            try
            {
                var user = await _service.GetUserDetails(id);

                return Ok(new BaseResponse<object>
                {
                    Success = true,
                    Message = "User details fetched successfully",
                    Data = user
                });
            }
            catch (Exception ex)
            {
                return NotFound(new BaseResponse<object>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] UserUpdateDto dto)
        {
            var validation = ValidationHelper.ValidateDto( dto);
            if (!validation.IsValid)
            {
                return BadRequest(new BaseResponse<object>
                {
                    Success = false,
                    Message = "Validation failed",
                    Errors = validation.Errors
                });
            }

            try
            {
                await _service.EditUser(id, dto);

                return Ok(new BaseResponse<object>
                {
                    Success = true,
                    Message = "User updated successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse<object>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id, [FromBody] UserDeleteDto dto)
        {
            try
            {
                await _service.DeleteUser(id, dto);

                return Ok(new BaseResponse<object>
                {
                    Success = true,
                    Message = "User deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse<object>
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }


    }
}
