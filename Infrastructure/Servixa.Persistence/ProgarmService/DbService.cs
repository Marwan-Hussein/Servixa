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
            return services;
        }
    }
}
