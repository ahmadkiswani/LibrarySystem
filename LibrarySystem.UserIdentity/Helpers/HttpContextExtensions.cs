using System.IdentityModel.Tokens.Jwt;

namespace LibrarySystem.UserIdentity.Helpers
{
    public static class HttpContextExtensions
    {
        public static int GetUserId(this HttpContext httpContext)
        {
            var claim = httpContext.User
                .FindFirst(JwtRegisteredClaimNames.Sub);

            if (claim == null)
                throw new UnauthorizedAccessException("UserId claim not found");

            return int.Parse(claim.Value);
        }
    }
}
