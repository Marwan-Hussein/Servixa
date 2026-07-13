using Microsoft.Extensions.Options;
using Servixa.Abstractions.Interfaces;
using Servixa.Abstractions.Settings;
using System.Net.Mail;
using MailKit.Net.Smtp;
using MimeKit;
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

            // to connect to the SMTP server and send the email
            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(_settings.SmtpServer, _settings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_settings.SenderEmail, _settings.Password);
            await client.SendAsync(msg);
            await client.DisconnectAsync(true);
        }
    }
}
