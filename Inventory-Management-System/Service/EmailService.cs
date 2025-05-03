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

        public async Task SendEmailsAsync(IQueryable<string> toEmails, string subject, string body)
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

            int size = 100;
            int skip = 0;
            List<string> emails;
            do
            {
                emails = toEmails.Skip(skip)
                    .Take(size)
                    .ToList();

                foreach (string e in emails)
                {
                    try
                    {
                        using var message = new MailMessage(fromEmail, e, subject, body);
                        await smtp.SendMailAsync(message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to send email to {e}: {ex.Message}");
                    }
                }

                skip += size;
            } while (emails.Any());
                        
        }
    }

}

