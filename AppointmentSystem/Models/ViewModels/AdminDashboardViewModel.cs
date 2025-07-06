using System.Collections.Generic;
using AppointmentSystem.Models;

namespace AppointmentSystem.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalAppointments { get; set; }
        public int PendingAppointments { get; set; }
        public int TotalEmployees { get; set; }
        public int TotalClients { get; set; }
        public List<Appointment> RecentAppointments { get; set; }
    }
} 