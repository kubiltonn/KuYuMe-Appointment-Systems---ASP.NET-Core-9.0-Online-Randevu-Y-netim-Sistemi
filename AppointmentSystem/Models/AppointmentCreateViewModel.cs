using System.ComponentModel.DataAnnotations;

namespace AppointmentSystem.Models
{
    public class AppointmentCreateViewModel
    {
        [Required(ErrorMessage = "Lütfen bir hizmet seçin")]
        [Display(Name = "Hizmet")]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Lütfen bir personel seçin")]
        [Display(Name = "Personel")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Lütfen randevu tarihi seçin")]
        [Display(Name = "Randevu Tarihi")]
        public DateTime AppointmentDate { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }
    }
} 