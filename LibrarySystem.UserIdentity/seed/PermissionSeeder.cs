using LibrarySystem.UserIdentity.Data;
using LibrarySystem.UserIdentity.Models;

namespace LibrarySystem.UserIdentity.Seed;

public static class PermissionSeeder
{
    public static async Task SeedAsync(IdentityDbContext db)
    {
        if (db.Permissions.Any()) return;

        var permissions = new[]
        {
            "book.create",
            "book.edit",
            "book.delete",
            "borrow.create",
            "borrow.return",
            "category.manage",
            "user.manage"
        };

        foreach (var p in permissions)
            db.Permissions.Add(new Permission { Name = p });

        await db.SaveChangesAsync();
    }
}
