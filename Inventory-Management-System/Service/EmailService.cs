using System.Net;
using System.Net.Mail;
using Inventory_Management_System.Service.Interfaces;

namespace Inventory_Management_System.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration config;

        public EmailService(IConfiguration config)
        {
            this.config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            string fromEmail = config["EmailSettings:FromEmail"];
            string password = config["EmailSettings:EmailPassword"];

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(fromEmail, password)
            };

            MailMessage message = new MailMessage(fromEmail, toEmail, subject, body);

            await smtp.SendMailAsync(message);
        }

        public async Task SendEmailsAsync(List<string> toEmails, string subject, string body)
        {
            string fromEmail = config["EmailSettings:FromEmail"];
            string password = config["EmailSettings:EmailPassword"];

            SmtpClient smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(fromEmail, password)
            };

            foreach (string toEmail in toEmails)
            {
                MailMessage message = new MailMessage(fromEmail, toEmail, subject, body);
                await smtp.SendMailAsync(message);
            }
        }
    }

}

