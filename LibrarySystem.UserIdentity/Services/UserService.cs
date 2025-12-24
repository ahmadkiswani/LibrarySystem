using LibrarySystem.UserIdentity.DTOs;
using LibrarySystem.UserIdentity.Iinterface;
using LibrarySystem.UserIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace LibrarySystem.UserIdentity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public UserService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

 
        public async Task RegisterAsync(RegisterDto dto)
        {
            var emailExists = await _userManager.FindByEmailAsync(dto.Email);
            if (emailExists != null)
                throw new Exception("Email already exists");

            var usernameExists = await _userManager.FindByNameAsync(dto.UserName);
            if (usernameExists != null)
                throw new Exception("Username already exists");

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                UserTypeId = dto.UserTypeId,
                IsArchived = false
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }
            await _userManager.AddToRoleAsync(user, "User");
        }

   
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            User? user;

            if (dto.UserNameOrEmail.Contains("@"))
                user = await _userManager.FindByEmailAsync(dto.UserNameOrEmail);
            else
                user = await _userManager.FindByNameAsync(dto.UserNameOrEmail);

            if (user == null)
                throw new Exception("Invalid credentials");

            if (user.IsArchived)
                throw new Exception("User is archived");

            var passwordValid = await _signInManager.CheckPasswordSignInAsync(
                user,
                dto.Password,
                false
            );

            if (!passwordValid.Succeeded)
                throw new Exception("Invalid credentials");

            var roles = await _userManager.GetRolesAsync(user);
            var token = GenerateJwtToken(user, roles);

            return new AuthResponseDto
            {
                UserId = user.Id,
                UserName = user.UserName ?? "",
                Token = token,
                Roles = roles.ToList()
            };
        }

   
        private string GenerateJwtToken(User user, IList<string> roles)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSection["Key"]!)
            );

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    int.Parse(jwtSection["ExpiresMinutes"]!)
                ),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
