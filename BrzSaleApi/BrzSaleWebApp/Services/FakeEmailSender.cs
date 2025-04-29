using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace BrzSaleWebApp.Services
{
    public class FakeEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Just return a completed task (no real email sending)
            Console.WriteLine($"Fake Email sent to {email}: {subject}");
            return Task.CompletedTask;
        }
    }
}