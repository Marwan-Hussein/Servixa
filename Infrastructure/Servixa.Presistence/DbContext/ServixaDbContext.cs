using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Servixa.Domain.Models.Users;

namespace Servixa.Presistence.DbContext
{
    public class ServixaDbContext(DbContextOptions<ServixaDbContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>(options)  
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.ApplyConfigurationsFromAssembly(typeof(ServixaDbContext).Assembly);
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
        }
    }
}
