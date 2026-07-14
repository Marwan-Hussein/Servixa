using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Servixa.Domain.Contracts.UnitOfWorkPattern;
using Servixa.Domain.Models.Users;
using Servixa.Presistence.DbContext;
using Servixa.Presistence.Implemntation.UnitOfWorkPattern;
using System.Text;

namespace Servixa.Presistence.ProgarmService
{
    public static class DbService
    {
        public static IServiceCollection InjectDbService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ServixaDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            // Identity Configuration
            services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ServixaDbContext>()
            .AddDefaultTokenProviders();

            // JWT Authentication
            var jwtSettings = configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            })
            .AddGoogle(options =>
            {
                options.ClientId = configuration["Authentication:Google:GoogleId"]!;
                options.ClientSecret = configuration["Authentication:Google:GoogleSecret"]!;
                options.SignInScheme = IdentityConstants.ExternalScheme;
            });

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
