using LibrarySystem.UserIdentity.Iinterface;
using LibrarySystem.UserIdentity.Models;
using LibrarySystem.UserIdentity.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly PermissionService _permissionService;

    public TokenService(
        IConfiguration config,
        PermissionService permissionService)
    {
        _config = config;
        _permissionService = permissionService;
    }

    public async Task<string> GenerateAccessToken(User user, IList<string> roles)
    {
        var jwt = _config.GetSection("Jwt");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName!)
        };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role));

        var permissions = await _permissionService.GetPermissionsForUser(user.Id);
        foreach (var p in permissions)
            claims.Add(new System.Security.Claims.Claim("permission", p));

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt["Key"]!)
        );

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials:
                new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
