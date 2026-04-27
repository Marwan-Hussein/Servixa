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
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
        }
    }
}
