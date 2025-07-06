using System.Collections.Generic;
using AppointmentSystem.Models;

namespace AppointmentSystem.Models.ViewModels
{
    public class ClientDetailsViewModel
    {
        public ApplicationUser Client { get; set; }
        public List<Appointment> Appointments { get; set; }
    }
} 