using Microsoft.AspNetCore.Identity;

namespace LibrarySystem.UserIdentity.Models
{
   
    public class User : IdentityUser<int>
    {
        public int UserTypeId { get; set; }
        public bool IsArchived { get; set; } = false;
    }
}

