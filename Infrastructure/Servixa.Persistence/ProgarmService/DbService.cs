using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Servixa.Domain.Models.Users;
using Servixa.Persistence.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servixa.Persistence.ProgramService
{
    public static class DbService
    {
        public static IServiceCollection InjectDbService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ServixaDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            /*services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ServixaDbContext>()
            .AddDefaultTokenProviders();*/

            return services;
        }
    }
}
