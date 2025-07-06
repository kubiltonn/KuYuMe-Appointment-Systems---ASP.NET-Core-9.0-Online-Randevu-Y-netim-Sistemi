using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AppointmentSystem.Data;
using AppointmentSystem.Models;
using System.Linq;

namespace AppointmentSystem.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationDbContext _context;

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAppointmentReminderAsync(Appointment appointment)
        {
            var notification = new Notification
            {
                UserId = appointment.CustomerId,
                Title = "Randevu Hatırlatması",
                Message = $"{appointment.AppointmentDate:dd/MM/yyyy HH:mm} tarihindeki {appointment.ServiceType} randevunuzu unutmayın.",
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
                Type = "AppointmentReminder",
                AppointmentId = appointment.Id
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetUnreadCountAsync(string userId)
        {
            return await _context.Notifications
                .CountAsync(n => n.UserId == userId && !n.IsRead);
        }
    }
} 