using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Text;
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

        public async Task SendVerificationCodeAsync(string email, string code)
        {

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.FromAddress));
            message.To.Add(new MailboxAddress("Пользователь Zap", email));

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"<h2>Здравствуйте! Ваш код подтверждения: <strong>{code}</strong></h2>" +
                           $"<p>Этот код действителен в течение 15 минут.</p>",
                TextBody = $"Ваш код подтверждения: {code}. Этот код действителен в течение 15 минут."
            };
            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);

                    await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);

                    await client.SendAsync(message);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"Не удалось отправить письмо на адрес {email}. Ошибка: {ex.Message}", ex);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}
