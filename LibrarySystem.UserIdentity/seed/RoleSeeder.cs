using LibrarySystem.UserIdentity.Data;
using LibrarySystem.UserIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.UserIdentity.Seed
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(
            RoleManager<IdentityRole<int>> roleManager,
            IdentityDbContext db)
        {
           var roles = new[] { "Admin", "Librarian", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(role));
                }
            }

            var permissions = new[]
            {
                "book.create",
                "book.update",
                "book.delete",
                "book.view",

                "borrow.create",
                "borrow.return",
                "borrow.view",

                "category.manage",
                "user.manage"
            };

            foreach (var perm in permissions)
            {
                if (!await db.Permissions.AnyAsync(p => p.Name == perm))
                {
                    db.Permissions.Add(new Permission
                    {
                        Name = perm
                    });
                }
            }

            await db.SaveChangesAsync();


            await Assign(db, "Admin", permissions);

            await Assign(db, "Librarian", new[]
            {
                "book.create",
                "book.update",
                "book.view",

                "borrow.create",
                "borrow.return",
                "borrow.view",

                "category.manage"
            });

            await Assign(db, "User", new[]
            {
                "book.view",
                "borrow.view"
            });
        }

        private static async Task Assign(IdentityDbContext db,string roleName,string[] permissions)
        {
            var role = await db.Roles.FirstAsync(r => r.Name == roleName);

            foreach (var permName in permissions)
            {
                var perm = await db.Permissions.FirstAsync(p => p.Name == permName);

                bool exists = await db.RolePermissions.AnyAsync(rp =>
                    rp.RoleId == role.Id &&
                    rp.PermissionId == perm.Id);

                if (!exists)
                {
                    db.RolePermissions.Add(new RolePermission
                    {
                        RoleId = role.Id,
                        PermissionId = perm.Id
                    });
                }
            }

            await db.SaveChangesAsync();
        }
    }
}
