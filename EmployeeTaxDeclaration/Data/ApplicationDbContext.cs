using EmployeeTaxDeclaration.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaxDeclaration.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options): base(options) { }

        public DbSet<ApplicationUser> AppUsers { get; set; }
        public DbSet<TaxForm> TaxForms { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var admin = new IdentityRole("admin");
            admin.NormalizedName = "admin";

            var client = new IdentityRole("client");
            client.NormalizedName = "client";

            builder.Entity<IdentityRole>().HasData(admin, client);

        }
    }
}
