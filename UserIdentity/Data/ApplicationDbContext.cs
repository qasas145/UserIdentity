using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace UserIdentity.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> builder)
        
            : base(builder)
        {  
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("Muhqasas");
            builder.Entity<ApplicationUser>().ToTable("Users", "Muhqasas");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<String>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<String>>().ToTable("UserClaims");
            builder.Entity<IdentityRoleClaim<String>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<String>>().ToTable("UserTokens");

            // ignoring columns 
            builder.Entity<ApplicationUser>().Ignore(u=>u.PhoneNumber);
            builder.Entity<ApplicationUser>().Ignore(u=>u.PhoneNumberConfirmed);
        }
    }
}
