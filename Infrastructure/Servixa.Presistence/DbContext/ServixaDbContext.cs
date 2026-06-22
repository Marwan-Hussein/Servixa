using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Servixa.Domain.Models.BookingEntity;
using Servixa.Domain.Models.NotificationEntity;
using Servixa.Domain.Models.PaymentEntity;
using Servixa.Domain.Models.ReviewEntity;
using Servixa.Domain.Models.SpecialtyEntity;
using Servixa.Domain.Models.Users;
using Servixa.Domain.Models;
using Servixa.Domain.Models.ChatEntity;

namespace Servixa.Presistence.DbContext
{
    public class ServixaDbContext(DbContextOptions<ServixaDbContext> options) : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>(options)  
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Servixa.Domain.Models.TaskEntity.Task> Tasks { get; set; }
        public DbSet<WorkerTask> WorkerTasks { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.ApplyConfigurationsFromAssembly(typeof(ServixaDbContext).Assembly);
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRole<int>>().ToTable("Roles");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
        }
    }
}
