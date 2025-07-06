using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentSystem.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı ID alanı zorunludur.")]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        [NotMapped]
        public virtual ApplicationUser User { get; set; } = null!;

        [Required(ErrorMessage = "Uzmanlık alanı zorunludur.")]
        [Display(Name = "Uzmanlık")]
        public string Expertise { get; set; } = string.Empty;

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Çalışma Saatleri")]
        public string? WorkingHours { get; set; }

        [Display(Name = "Aktif mi?")]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public DateTime? UpdatedAt { get; set; }

        [NotMapped]
        public virtual ICollection<Service> Services { get; set; } = new List<Service>();
        [NotMapped]
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
} 