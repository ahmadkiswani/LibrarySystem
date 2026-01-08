using Microsoft.AspNetCore.Identity;

namespace LibrarySystem.UserIdentity.Models
{
   
    public class User : IdentityUser<int>
    {
        public bool IsArchived { get; set; } = false;
    }
}

