using MailKit.Net.Smtp;
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

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
            msg.To.Add(new MailboxAddress("", to));
            msg.Subject = subject;
            msg.Body = new TextPart("html")
            {
                Text = body
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_settings.SmtpServer, _settings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_settings.SenderEmail, _settings.Password);
            await client.SendAsync(msg);
            await client.DisconnectAsync(true);
        }

        public async Task SendOtpEmailAsync(string to, string displayName, string otpCode, DateTime expiresAt)
        {
            var body = EmailTemplateHelper.BuildOtpTemplate(displayName, otpCode, expiresAt);
            await SendAsync(to, "Your Servixa verification code", body);
        }
    }
}
