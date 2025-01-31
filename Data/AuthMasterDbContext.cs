using masterapi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace masterapi.Data
{
    public class AuthMasterDbContext : IdentityDbContext
    {
        public AuthMasterDbContext(DbContextOptions<AuthMasterDbContext> options) : base(options)
        {
        }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Village> Villages { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<District> Districts { get; set; }

        public DbSet<Bank> Banks { get; set; }

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

