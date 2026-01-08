using LibrarySystem.UserIdentity.Models;

namespace LibrarySystem.UserIdentity.Iinterface
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(User user, IList<string> roles);
    }
}
