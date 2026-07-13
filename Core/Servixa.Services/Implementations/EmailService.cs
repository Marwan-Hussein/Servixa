using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Servixa.Abstractions.Interfaces;
using Servixa.Abstractions.Settings;
using Servixa.Services.Helpers;

namespace Servixa.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> settings, ILogger<EmailService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            ValidateSettings();

            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            msg.To.Add(new MailboxAddress("", to));
            msg.Subject = subject;
            msg.Body = new TextPart("html")
            {
                Text = body
            };

            using var client = new SmtpClient();
            var socketOptions = _settings.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls;

            try
            {
                await client.ConnectAsync(_settings.SmtpServer, _settings.Port, socketOptions);
                await client.AuthenticateAsync(_settings.SenderEmail, _settings.Password);
                await client.SendAsync(msg);
                await client.DisconnectAsync(true);

                _logger.LogInformation("Email with subject '{Subject}' was sent to {Recipient}.", subject, to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email with subject '{Subject}' to {Recipient}.", subject, to);
                throw new Exception("Failed to send verification email. Please check SMTP settings and try again.", ex);
            }
        }

        public async Task SendOtpEmailAsync(string to, string displayName, string otpCode, DateTime expiresAt)
        {
            var body = EmailTemplateHelper.BuildOtpTemplate(displayName, otpCode, expiresAt);
            await SendAsync(to, "Your Servixa verification code", body);
        }

        private void ValidateSettings()
        {
            if (string.IsNullOrWhiteSpace(_settings.SmtpServer))
                throw new Exception("EmailSettings:SmtpServer is missing.");

            if (_settings.Port <= 0)
                throw new Exception("EmailSettings:Port is missing or invalid.");

            if (string.IsNullOrWhiteSpace(_settings.SenderEmail))
                throw new Exception("EmailSettings:SenderEmail is missing.");

            if (string.IsNullOrWhiteSpace(_settings.SenderName))
                throw new Exception("EmailSettings:SenderName is missing.");

            if (string.IsNullOrWhiteSpace(_settings.Password))
                throw new Exception("EmailSettings:Password is missing.");
        }
    }
}
