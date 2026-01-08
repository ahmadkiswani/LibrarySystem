using LibrarySystem.UserIdentity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.UserIdentity.Seed;

public static class RolePermissionSeeder
{
    public static async Task SeedAsync(
        IdentityDbContext db,
        RoleManager<IdentityRole<int>> roleManager)
    {
        var admin = await roleManager.FindByNameAsync("Admin");
        var librarian = await roleManager.FindByNameAsync("Librarian");
        var user = await roleManager.FindByNameAsync("User");

        var permissions = await db.Permissions.ToListAsync();

        void Assign(IdentityRole<int> role, params string[] perms)
        {
            foreach (var p in permissions.Where(x => perms.Contains(x.Name)))
            {
                if (!db.RolePermissions.Any(rp =>
                        rp.RoleId == role.Id && rp.PermissionId == p.Id))
                {
                    db.RolePermissions.Add(new()
                    {
                        RoleId = role.Id,
                        PermissionId = p.Id
                    });
                }
            }
        }

        Assign(admin, permissions.Select(p => p.Name).ToArray());

        Assign(librarian,
            "book.create",
            "book.edit",
            "borrow.create",
            "borrow.return",
            "category.manage");

        Assign(user,
            "borrow.create",
            "borrow.return");

        await db.SaveChangesAsync();
    }
}
