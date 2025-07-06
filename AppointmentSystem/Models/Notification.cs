using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentSystem.Models
{
    public class Notification
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kullanıcı ID alanı zorunludur.")]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("UserId")]
        [NotMapped]
        public virtual ApplicationUser User { get; set; } = null!;

        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        [Display(Name = "Başlık")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mesaj alanı zorunludur.")]
        [Display(Name = "Mesaj")]
        public string Message { get; set; } = string.Empty;

        [Display(Name = "Okundu")]
        public bool IsRead { get; set; }

        [Display(Name = "Okunma Tarihi")]
        public DateTime? ReadAt { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; } = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        [Required(ErrorMessage = "Bildirim tipi alanı zorunludur.")]
        [Display(Name = "Bildirim Tipi")]
        public string Type { get; set; } = string.Empty;

        [Display(Name = "Randevu")]
        public int? AppointmentId { get; set; }

        [ForeignKey("AppointmentId")]
        [NotMapped]
        public virtual Appointment? Appointment { get; set; }
    }
} 