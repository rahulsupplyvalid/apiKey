using api_key_Authorize.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api_key_Authorize.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Define your models (tables)
        public DbSet<ProtectedData> ProtectedData { get; set; }




        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Hardcoded static role IDs
            var adminRoleId = "ADM20250115093045"; // Static role ID
            var userRoleId = "USE20250115093045";  // Static role ID

            // Define roles
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                },
                new IdentityRole
                {
                    Id = userRoleId,
                    ConcurrencyStamp = userRoleId,
                    Name = "User",
                    NormalizedName = "User".ToUpper()
                }
            };

            // Seed roles
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
