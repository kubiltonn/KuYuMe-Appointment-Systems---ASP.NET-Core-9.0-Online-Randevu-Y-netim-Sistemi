using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using AppointmentSystem.Data;
using AppointmentSystem.Models;

namespace AppointmentSystem.Services
{
    public class AppointmentReminderService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<AppointmentReminderService> _logger;

        public AppointmentReminderService(
            IServiceProvider services,
            ILogger<AppointmentReminderService> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await CheckUpcomingAppointmentsAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Randevu hatırlatma kontrolü sırasında bir hata oluştu.");
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        private async Task CheckUpcomingAppointmentsAsync()
        {
            using var scope = _services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

            var now = DateTime.UtcNow;
            var tomorrow = now.AddDays(1);

            var upcomingAppointments = await context.Appointments
                .Include(a => a.Service)
                .Where(a => a.AppointmentDate >= now && 
                           a.AppointmentDate <= tomorrow && 
                           a.Status == AppointmentStatus.Confirmed)
                .ToListAsync();

            foreach (var appointment in upcomingAppointments)
            {
                await notificationService.CreateAppointmentReminderAsync(appointment);
            }
        }
    }
} 