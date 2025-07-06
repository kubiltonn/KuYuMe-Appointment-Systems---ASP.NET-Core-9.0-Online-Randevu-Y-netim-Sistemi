using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppointmentSystem.Models;

namespace AppointmentSystem.Models.ViewModels
{
    public class AppointmentCreateViewModel
    {
        [Required(ErrorMessage = "Hizmet alanı zorunludur.")]
        [Display(Name = "Hizmet")]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Personel alanı zorunludur.")]
        [Display(Name = "Personel")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Randevu tarihi alanı zorunludur.")]
        [Display(Name = "Randevu Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime AppointmentDate { get; set; }

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        public List<SelectListItem> Services { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Employees { get; set; } = new List<SelectListItem>();
    }
} 