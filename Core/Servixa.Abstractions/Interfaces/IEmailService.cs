namespace Servixa.Abstractions.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string body);
        Task SendOtpEmailAsync(string to, string displayName, string otpCode, DateTime expiresAt);
    }
}
