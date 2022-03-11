using EmailSender.Models;
using EmailSender.Services.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PasswordManager.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailSenderService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SenderEmailAsync(string recipiantEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(MailboxAddress.Parse(_smtpSettings.SenderEmail));
            message.To.Add(MailboxAddress.Parse(recipiantEmail));
            message.Subject = subject;
            message.Body = new TextPart("Plain")
            {
                Text = body
            };
            var client = new SmtpClient();
            try
            {
                client.Connect(_smtpSettings.Server, _smtpSettings.Port, true);
                client.Authenticate(new NetworkCredential(_smtpSettings.SenderEmail, _smtpSettings.Password));
                await client.SendAsync(message);
                client.Disconnect(true);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                client.Dispose();
            }
        }
    }
}

