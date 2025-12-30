using LibrarySystem.UserIdentity.DTOs;
using LibrarySystem.UserIdentity.Iinterface;
using LibrarySystem.UserIdentity.Models;
using Microsoft.AspNetCore.Identity;
using MassTransit;
using LibrarySystem.Common.Messaging;

namespace LibrarySystem.UserIdentity.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IPublishEndpoint _publishEndpoint;

    public UserService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ITokenService tokenService,
        IPublishEndpoint publishEndpoint)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task RegisterAsync(RegisterDto dto)
    {
        var user = new User
        {
            UserName = dto.UserName,
            Email = dto.Email,
            UserTypeId = dto.UserTypeId
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

        await _userManager.AddToRoleAsync(user, "User");

        await _publishEndpoint.Publish(new UserCreatedMessage
        {
            UserId = user.Id,
            UserName = user.UserName!,
            Email = user.Email!,
            UserTypeId = user.UserTypeId,
            OccurredAt = DateTime.UtcNow
        }, ctx =>
        {
            ctx.SetRoutingKey(LibraryRoutingKeys.UserCreated);
        });
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var user = dto.UserNameOrEmail.Contains("@")
            ? await _userManager.FindByEmailAsync(dto.UserNameOrEmail)
            : await _userManager.FindByNameAsync(dto.UserNameOrEmail);

        if (user == null || user.IsArchived)
            throw new UnauthorizedAccessException("Invalid credentials");

        var ok = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!ok.Succeeded)
            throw new UnauthorizedAccessException("Invalid credentials");

        var roles = await _userManager.GetRolesAsync(user);
        var token = _tokenService.GenerateAccessToken(user, roles);

        return new AuthResponseDto
        {
            UserId = user.Id,
            UserName = user.UserName ?? string.Empty,
            Token = token,
            Roles = roles.ToList()
        };
    }

    public async Task UpdateAsync(UpdateUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId.ToString())
                   ?? throw new Exception("User not found");

        user.UserName = dto.UserName;
        user.Email = dto.Email;
        user.UserTypeId = dto.UserTypeId;

        await _userManager.UpdateAsync(user);

        await _publishEndpoint.Publish(new UserUpdatedMessage
        {
            UserId = user.Id,
            UserName = user.UserName!,
            Email = user.Email!,
            UserTypeId = user.UserTypeId,
            OccurredAt = DateTime.UtcNow
        }, ctx =>
        {
            ctx.SetRoutingKey(LibraryRoutingKeys.UserUpdated);
        });
    }

    public async Task DeactivateAsync(DeactivateUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(dto.UserId.ToString())
                   ?? throw new Exception("User not found");

        user.IsArchived = true;
        await _userManager.UpdateAsync(user);

        await _publishEndpoint.Publish(new UserDeactivatedMessage
        {
            UserId = user.Id,
            OccurredAt = DateTime.UtcNow
        }, ctx =>
        {
            ctx.SetRoutingKey(LibraryRoutingKeys.UserDeactivated);
        });
    }
}
