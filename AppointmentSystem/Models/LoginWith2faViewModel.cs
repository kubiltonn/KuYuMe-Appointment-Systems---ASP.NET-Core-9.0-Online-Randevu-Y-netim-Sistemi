using System.ComponentModel.DataAnnotations;

namespace AppointmentSystem.Models
{
    public class LoginWith2faViewModel
    {
        [Required(ErrorMessage = "Doğrulama kodu zorunludur.")]
        [StringLength(7, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Doğrulama Kodu")]
        public string TwoFactorCode { get; set; } = string.Empty;

        [Display(Name = "Bu tarayıcıyı hatırla")]
        public bool RememberMachine { get; set; }

        public bool RememberMe { get; set; }
    }
} 