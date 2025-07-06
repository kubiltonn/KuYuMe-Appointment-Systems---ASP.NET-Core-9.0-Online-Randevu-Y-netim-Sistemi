using System.Threading.Tasks;
using AppointmentSystem.Models;

namespace AppointmentSystem.Services
{
    public interface INotificationService
    {
        Task CreateAppointmentReminderAsync(Appointment appointment);
        Task MarkAsReadAsync(int notificationId);
        Task<int> GetUnreadCountAsync(string userId);
    }
} 