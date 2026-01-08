using Microsoft.AspNetCore.Identity;

namespace LibrarySystem.UserIdentity.Models;

public class RolePermission
{
    public int RoleId { get; set; }
    public IdentityRole<int> Role { get; set; } = null!;

    public int PermissionId { get; set; }
    public Permission Permission { get; set; } = null!;
}
