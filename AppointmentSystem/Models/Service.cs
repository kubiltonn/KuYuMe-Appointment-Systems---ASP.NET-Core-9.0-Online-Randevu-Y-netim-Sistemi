using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentSystem.Models
{
    public class Service
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Hizmet adı alanı zorunludur.")]
        [Display(Name = "Hizmet Adı")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Display(Name = "Süre (Dakika)")]
        public int? Duration { get; set; }

        [Display(Name = "Fiyat")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }

        [Display(Name = "Aktif mi?")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Üst Hizmet")]
        public int? ParentServiceId { get; set; }

        [ForeignKey("ParentServiceId")]
        public virtual Service? ParentService { get; set; }

        public virtual ICollection<Service> SubServices { get; set; } = new List<Service>();
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; } = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        [Display(Name = "Güncellenme Tarihi")]
        public DateTime? UpdatedAt { get; set; }
    }
} 