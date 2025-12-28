using LibrarySystem.UserIdentity.DTOs;
using LibrarySystem.UserIdentity.Helpers;
using LibrarySystem.UserIdentity.Iinterface;
using LibrarySystem.UserIdentity.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.UserIdentity.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var validationErrors = ValidationHelper.Validate(dto);
            if (validationErrors.Any())
            {
                return BadRequest(
                    BaseResponse<string>.FailureResponse(
                        "Validation failed",
                        validationErrors
                    )
                );
            }

            try
            {
                await _userService.RegisterAsync(dto);

                return Ok(
                    BaseResponse<string>.SuccessResponse(
                        null!,
                        "User registered successfully"
                    )
                );
            }
            catch (Exception ex)
            {
                return BadRequest(
                    BaseResponse<string>.FailureResponse(
                        ex.Message
                    )
                );
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var validationErrors = ValidationHelper.Validate(dto);
            if (validationErrors.Any())
            {
                return BadRequest(
                    BaseResponse<string>.FailureResponse(
                        "Validation failed",
                        validationErrors
                    )
                );
            }

            try
            {
                var result = await _userService.LoginAsync(dto);

                return Ok(
                    BaseResponse<AuthResponseDto>.SuccessResponse(
                        result,
                        "Login successful"
                    )
                );
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(
                    BaseResponse<string>.FailureResponse(
                        ex.Message
                    )
                );
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("deactivate")]
        public async Task<IActionResult> Deactivate([FromBody] DeactivateUserDto dto)
        {
            var validationErrors = ValidationHelper.Validate(dto);
            if (validationErrors.Any())
            {
                return BadRequest(
                    BaseResponse<string>.FailureResponse(
                        "Validation failed",
                        validationErrors
                    )
                );
            }

            try
            {
                await _userService.DeactivateAsync(dto);

                return Ok(
                    BaseResponse<string>.SuccessResponse(
                        null!,
                        "User deactivated successfully"
                    )
                );
            }
            catch (Exception ex)
            {
                return BadRequest(
                    BaseResponse<string>.FailureResponse(
                        ex.Message
                    )
                );
            }
        }
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateUserDto dto)
        {
            var validationErrors = ValidationHelper.Validate(dto);
            if (validationErrors.Any())
            {
                return BadRequest(
                    BaseResponse<string>.FailureResponse(
                        "Validation failed",
                        validationErrors
                    )
                );
            }

            try
            {
                await _userService.UpdateAsync(dto);

                return Ok(
                    BaseResponse<string>.SuccessResponse(
                        null!,
                        "User updated successfully"
                    )
                );
            }
            catch (Exception ex)
            {
                return BadRequest(
                    BaseResponse<string>.FailureResponse(ex.Message)
                );
            }
        }
      



    }
}
