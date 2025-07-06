using System.Threading.Tasks;

namespace AppointmentSystem.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
} 