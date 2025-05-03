namespace Inventory_Management_System.Service.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string toEmail, string subject, string body);
        public Task SendEmailsAsync(IQueryable<string> toEmails, string subject, string body);
    }
}