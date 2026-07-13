
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Servixa.Domain.Models.Users;
using Servixa.Presistence.ProgarmService;
using Servixa.Web.Extensions;
using Servixa.Web.Middleware;

namespace Servixa.Web
{
    public class Program
    {
        public static async System.Threading.Tasks.Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // remote frontend
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Frontend", policy =>
                {
                    policy
                        .WithOrigins("https://servixa-two.vercel.app")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials(); // only if using cookies or SignalR
                });
            });

            // Add services to the container.

            builder.Services.AddControllers();
            
            // Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
            //injecting database services
            builder.Services.InjectDbService(builder.Configuration);
            //injecting application services
            builder.Services.InjectApplicationService(builder.Configuration);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. Just paste your token directly here, the 'Bearer ' prefix will be added automatically."
                });

                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            // Seed Database
            /*using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<Servixa.Presistence.DbContext.ServixaDbContext>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
                    
                    await ServixaDbSeeder.SeedAsync(roleManager, userManager, context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred during database seeding.");
                }
            }*/

            app.UseMiddleware<GlobalErrorHandlerMiddleware>();
            // Enable Swagger in all environments (useful for production testing)
            app.UseSwagger();
            app.UseSwaggerUI();

            // Optional: Redirect the root URL "/" to Swagger UI to prevent 404 errors
            app.MapGet("/", () => Results.Redirect("/swagger/index.html"));

            app.UseHttpsRedirection();
            
            app.UseStaticFiles(); // For wwwroot/uploads
            
            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}
