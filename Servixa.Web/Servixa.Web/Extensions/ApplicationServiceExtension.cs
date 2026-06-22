using Servixa.Abstractions.Interfaces;
using Servixa.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Servixa.Web.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection InjectApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<ISpecialtyService, SpecialtyService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<IWorkerService, WorkerService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IPaymentService, PaymentService>();
            
            return services;
        }
    }
}
