using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System;
using System.Threading.Tasks;
using Zap.BLL.Infrastructure;
using Zap.BLL.Interfaces;

namespace Zap.BLL.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendVerificationCodeAsync(string recipientEmail, string code)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            message.To.Add(new MailboxAddress("", recipientEmail));
            message.Subject = "Код подтверждения Zap";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                    <h2>Здравствуйте!</h2>
                    <p>Ваш код подтверждения: <strong>{code}</strong></p>
                    <p>Он действителен в течение <strong>15 минут</strong>.</p>",
                TextBody = $"Ваш код подтверждения: {code}. Он действителен в течение 15 минут."
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
                await client.SendAsync(message);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Ошибка при отправке письма на {recipientEmail}: {ex.Message}", ex);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}
