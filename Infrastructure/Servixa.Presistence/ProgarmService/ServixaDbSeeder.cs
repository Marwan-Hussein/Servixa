using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Servixa.Domain.Models.Users;
using Servixa.Domain.Models.SpecialtyEntity;
using Servixa.Domain.Models.TaskEntity;

namespace Servixa.Presistence.ProgarmService
{
    public static class ServixaDbSeeder
    {
        public static async System.Threading.Tasks.Task SeedAsync(RoleManager<IdentityRole<int>> roleManager, UserManager<ApplicationUser> userManager, DbContext.ServixaDbContext context)
        {
            // Seed Roles
            var roles = new[] { "Admin", "Client", "Worker" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<int>(role));
                }
            }

            // Seed Admin User
            if (await userManager.FindByEmailAsync("admin@servixa.com") == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin@servixa.com",
                    Email = "admin@servixa.com",
                    FirstName = "System",
                    LastName = "Admin",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            // Seed Specialties and Tasks
            if (!await context.Specialties.AnyAsync())
            {
                var specialties = new List<Specialty>
                {
                    new Specialty 
                    { 
                        Name = "Plumbing", 
                        Description = "Pipe and water systems",
                        Tasks = new List<Servixa.Domain.Models.TaskEntity.Task>
                        {
                            new Servixa.Domain.Models.TaskEntity.Task { Name = "Fix Leaking Pipe", AvgCost = 50, AvgTime = DateTime.UtcNow.Date.AddHours(1) },
                            new Servixa.Domain.Models.TaskEntity.Task { Name = "Install Sink", AvgCost = 100, AvgTime = DateTime.UtcNow.Date.AddHours(2) }
                        }
                    },
                    new Specialty 
                    { 
                        Name = "Electrical", 
                        Description = "Wiring and electrical systems",
                        Tasks = new List<Servixa.Domain.Models.TaskEntity.Task>
                        {
                            new Servixa.Domain.Models.TaskEntity.Task { Name = "Install Ceiling Fan", AvgCost = 80, AvgTime = DateTime.UtcNow.Date.AddHours(1.5) },
                            new Servixa.Domain.Models.TaskEntity.Task { Name = "Fix Outlet", AvgCost = 40, AvgTime = DateTime.UtcNow.Date.AddMinutes(30) }
                        }
                    }
                };

                await context.Specialties.AddRangeAsync(specialties);
                await context.SaveChangesAsync();
            }
        }
    }
}
