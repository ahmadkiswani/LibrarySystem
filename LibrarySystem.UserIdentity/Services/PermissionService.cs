using LibrarySystem.UserIdentity.Data;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.UserIdentity.Services;

public interface IPermissionService
{
    Task<List<string>> GetPermissionsForUser(int userId);
}

public class PermissionService : IPermissionService
{
    private readonly IdentityDbContext _db;

    public PermissionService(IdentityDbContext db)
    {
        _db = db;
    }

    public async Task<List<string>> GetPermissionsForUser(int userId)
    {
        var roleIds = await _db.UserRoles
            .Where(x => x.UserId == userId)
            .Select(x => x.RoleId)
            .ToListAsync();

        return await _db.RolePermissions
            .Where(rp => roleIds.Contains(rp.RoleId))
            .Select(rp => rp.Permission.Name)
            .Distinct()
            .ToListAsync();
    }
}
