using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastrucure.Persistence.Seed
{
    public class SeedInitialData
    {
        public static void SuperAdmin(ModelBuilder builder)
        {
            const int ADMIN_ID = 1;
            const int ROLE_ID = ADMIN_ID;
            builder.Entity<AuthRole>().HasData(new AuthRole
            {
                Id = ROLE_ID,
                Name = "SuperAdmin",
                NormalizedName = "SuperAdmin"
            });

            var hasher = new PasswordHasher<Users>();

            builder.Entity<Users>().HasData(new Users
            {
                Id = ADMIN_ID,
                NormalizedUserName = "admin@admin.com",
                UserName = "admin@admin.com",
                NormalizedEmail = "admin@admin.com",
                Email = "admin@admin.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "Sun@123"),
                SecurityStamp = string.Empty,
                LockoutEnabled = false,
                Dob = DateTime.Now,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = ADMIN_ID,
                CreatedOnUtc = DateTime.UtcNow,
                UpdatedBy = ADMIN_ID,
                UpdatedOnUtc = DateTime.UtcNow
            });

            builder.Entity<IdentityUserRole<int>>().HasData(new IdentityUserRole<int>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID
            });
        }

        public static void Roles(ModelBuilder builder)
        {
            builder.Entity<AuthRole>().HasData(new AuthRole
            {
                Id = 2,
                Name = "SubAdmin",
                NormalizedName = "SubAdmin"
            });
        }
    }
}
