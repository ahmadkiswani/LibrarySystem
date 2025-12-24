using LibrarySystem.UserIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.UserIdentity.Data
{
    public class IdentityDbContext
        : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(entity =>
            {
                entity.Property(u => u.UserTypeId)
                      .IsRequired();

                entity.Property(u => u.IsArchived)
                      .HasDefaultValue(false);
            });
        }
    }
}
