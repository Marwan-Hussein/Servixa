using Servixa.Domain.Models.Users;

namespace Servixa.Abstractions.Interfaces
{
    public interface IOtpService
    {
        Task SendRegistrationOtpAsync(ApplicationUser user);
        Task VerifyRegistrationOtpAsync(string email, string code);
    }
}
