using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppointmentSystem.Models;

namespace AppointmentSystem.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public string CustomerId { get; set; }
        [NotMapped]
        public ApplicationUser Customer { get; set; }

        [Required]
        public int ServiceId { get; set; }
        [NotMapped]
        public Service Service { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        [NotMapped]
        public Employee Employee { get; set; }

        [Required]
        [Display(Name = "Randevu Tarihi")]
        public DateTime AppointmentDate { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Durum")]
        public AppointmentStatus Status { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; } = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        [Display(Name = "Güncellenme Tarihi")]
        public DateTime? UpdatedAt { get; set; }

        [NotMapped]
        public string CustomerName => Customer?.Name;

        [NotMapped]
        public string PhoneNumber => Customer?.PhoneNumber;

        [NotMapped]
        public string Region => Customer?.Region;

        [NotMapped]
        public string EmployeeName => Employee?.User?.Name;

        [NotMapped]
        public string ServiceType => Service?.Name;
    }

    public enum AppointmentStatus
    {
        [Display(Name = "Beklemede")]
        Pending,
        [Display(Name = "Onaylandı")]
        Confirmed,
        [Display(Name = "İptal Edildi")]
        Cancelled,
        [Display(Name = "Tamamlandı")]
        Completed,
        [Display(Name = "Reddedildi")]
        Rejected
    }
} 