using System.Threading.Tasks;

namespace EmailSender.Services.Interfaces
{
    public interface IEmailSenderService
    {
        Task SenderEmailAsync(string recipiantEmail, string subject, string body);
    }
}
