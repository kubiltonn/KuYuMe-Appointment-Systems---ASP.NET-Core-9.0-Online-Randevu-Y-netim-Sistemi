using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AppointmentSystem.Data;
using AppointmentSystem.Models;
using AppointmentSystem.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AppointmentSystem.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard()
        {
            ViewBag.Notifications = new List<string> { "Yeni randevu talebi geldi.", "Bir çalışan profilini güncelledi.", "Sistem yedeği başarıyla alındı." };
            ViewBag.Messages = new List<string> { "Hoş geldiniz, yönetici!", "Yeni bir mesajınız var.", "Destek ekibiyle iletişime geçin." };
            
            var model = new AdminDashboardViewModel
            {
                TotalAppointments = await _context.Appointments.CountAsync(),
                PendingAppointments = await _context.Appointments.Where(a => a.Status == AppointmentStatus.Pending).CountAsync(),
                TotalEmployees = await _userManager.GetUsersInRoleAsync("Employee") is var employees ? employees.Count : 0,
                TotalClients = await _userManager.GetUsersInRoleAsync("Client") is var clients ? clients.Count : 0,
                RecentAppointments = await _context.Appointments
                    .Include(a => a.Customer)
                    .Include(a => a.Service)
                    .Include(a => a.Employee)
                        .ThenInclude(e => e.User)
                    .OrderByDescending(a => a.AppointmentDate)
                    .Take(5)
                    .ToListAsync()
            };

            return View(model);
        }

        public async Task<IActionResult> Index()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Employee)
                .Include(a => a.Service)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return View(appointments);
        }

        public async Task<IActionResult> Appointments()
        {
            ViewBag.Notifications = new List<string> { "Yeni randevu talebi geldi.", "Bir çalışan profilini güncelledi.", "Sistem yedeği başarıyla alındı." };
            ViewBag.Messages = new List<string> { "Hoş geldiniz, yönetici!", "Yeni bir mesajınız var.", "Destek ekibiyle iletişime geçin." };
            var appointments = await _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Employee)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return View(appointments);
        }

        public async Task<IActionResult> Employees()
        {
            ViewBag.Notifications = new List<string> { "Yeni randevu talebi geldi.", "Bir çalışan profilini güncelledi.", "Sistem yedeği başarıyla alındı." };
            ViewBag.Messages = new List<string> { "Hoş geldiniz, yönetici!", "Yeni bir mesajınız var.", "Destek ekibiyle iletişime geçin." };
            var employees = await _userManager.GetUsersInRoleAsync("Employee");
            return View(employees);
        }

        public async Task<IActionResult> Clients()
        {
            ViewBag.Notifications = new List<string> { "Yeni randevu talebi geldi.", "Bir çalışan profilini güncelledi.", "Sistem yedeği başarıyla alındı." };
            ViewBag.Messages = new List<string> { "Hoş geldiniz, yönetici!", "Yeni bir mesajınız var.", "Destek ekibiyle iletişime geçin." };
            var clients = await _userManager.GetUsersInRoleAsync("Client");
            return View(clients);
        }

        public IActionResult Settings()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAppointmentStatus(int id, AppointmentStatus status)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Status = status;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Appointments));
        }

        [HttpGet]
        public async Task<IActionResult> EditAppointment(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Employee)
                    .ThenInclude(e => e.User)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            // Müşteri listesini yükle
            var customers = await _userManager.GetUsersInRoleAsync("Client");
            ViewBag.Customers = new SelectList(customers, "Id", "FullName", appointment.CustomerId);

            // Hizmet listesini yükle
            var services = await _context.Services
                .Where(s => s.ParentServiceId != null)
                .ToListAsync();
            ViewBag.Services = new SelectList(services, "Id", "Name", appointment.ServiceId);

            // Personel listesini yükle
            var employees = await _context.Employees
                .Include(e => e.User)
                .Where(e => e.IsActive)
                .ToListAsync();
            ViewBag.Employees = new SelectList(employees, "Id", "User.FullName", appointment.EmployeeId);

            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAppointment(int id, [Bind("Id,CustomerId,EmployeeId,ServiceId,AppointmentDate,Description,Status,CreatedAt")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                TempData["FormErrors"] = "Geçersiz randevu ID'si.";
                await LoadViewBagData();
                return View(appointment);
            }

            // ModelState'den NotMapped alanlarını temizle
            ModelState.Remove("Customer");
            ModelState.Remove("Service");
            ModelState.Remove("Employee");
            ModelState.Remove("CustomerName");
            ModelState.Remove("PhoneNumber");
            ModelState.Remove("Region");
            ModelState.Remove("EmployeeName");
            ModelState.Remove("ServiceType");

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                TempData["FormErrors"] = string.Join("<br>", errors);
                await LoadViewBagData();
                return View(appointment);
            }

            var existingAppointment = await _context.Appointments.FindAsync(id);
            if (existingAppointment == null)
            {
                TempData["FormErrors"] = "Randevu bulunamadı.";
                await LoadViewBagData();
                return View(appointment);
            }

            // Güncelle
            existingAppointment.CustomerId = appointment.CustomerId;
            existingAppointment.EmployeeId = appointment.EmployeeId;
            existingAppointment.ServiceId = appointment.ServiceId;
            existingAppointment.AppointmentDate = appointment.AppointmentDate;
            existingAppointment.Description = appointment.Description;
            existingAppointment.Status = appointment.Status;
            existingAppointment.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Randevu başarıyla güncellendi.";
                return RedirectToAction(nameof(Appointments));
            }
            catch (Exception ex)
            {
                TempData["FormErrors"] = "Randevu güncellenirken bir hata oluştu: " + ex.Message;
                await LoadViewBagData();
                return View(appointment);
            }
        }

        private async Task LoadViewBagData()
        {
            ViewBag.Employees = new SelectList(await _context.Employees
                .Include(e => e.User)
                .ToListAsync(), "Id", "User.FullName");
            
            ViewBag.Services = new SelectList(await _context.Services
                .Where(s => s.ParentServiceId != null)
                .ToListAsync(), "Id", "Name");

            ViewBag.Customers = new SelectList(await _userManager.GetUsersInRoleAsync("Client"), "Id", "FullName");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Appointments));
        }

        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEmployee([Bind("FullName,Email,PhoneNumber,Specialization")] ApplicationUser employee)
        {
            try
            {
                if (string.IsNullOrEmpty(employee.FullName))
                    return Json(new { success = false, message = "Ad Soyad alanı boş olamaz." });
                
                if (string.IsNullOrEmpty(employee.Email))
                    return Json(new { success = false, message = "E-posta alanı boş olamaz." });
                
                if (string.IsNullOrEmpty(employee.PhoneNumber))
                    return Json(new { success = false, message = "Telefon alanı boş olamaz." });
                
                if (string.IsNullOrEmpty(employee.Specialization))
                    return Json(new { success = false, message = "Uzmanlık alanı boş olamaz." });

                // E-posta adresi zaten kullanılıyor mu kontrol et
                var existingUser = await _userManager.FindByEmailAsync(employee.Email);
                if (existingUser != null)
                {
                    return Json(new { success = false, message = "Bu e-posta adresi zaten kullanılıyor." });
                }

                employee.UserName = employee.Email;
                employee.EmailConfirmed = true; // E-posta onayını otomatik yap
                employee.PhoneNumberConfirmed = true; // Telefon onayını otomatik yap

                // Geçici şifre oluştur
                var tempPassword = "Employee123!";
                var result = await _userManager.CreateAsync(employee, tempPassword);

                if (result.Succeeded)
                {
                    // Employee rolünü ekle
                    await _userManager.AddToRoleAsync(employee, "Employee");

                    // Employee tablosuna kayıt ekle
                    var newEmployee = new Employee
                    {
                        UserId = employee.Id,
                        Expertise = employee.Specialization,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.Employees.Add(newEmployee);
                    await _context.SaveChangesAsync();

                    // Kullanıcıya bilgilendirme mesajı gönder
                    var message = $"Hesabınız başarıyla oluşturuldu.\nE-posta: {employee.Email}\nŞifre: {tempPassword}\nLütfen giriş yaptıktan sonra şifrenizi değiştirin.";
                    TempData["SuccessMessage"] = message;

                    return Json(new { success = true, message = "Çalışan başarıyla eklendi. Giriş bilgileri: " + message });
                }
                
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return Json(new { success = false, message = errorMessage });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Çalışan eklenirken bir hata oluştu: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployee(string id)
        {
            var employee = await _userManager.FindByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            await LoadViewBagData();
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployee(string id, [Bind("Id,FullName,Email,PhoneNumber,Specialization")] ApplicationUser employee)
        {
            try
            {
                // ModelState'den gereksiz alanları temizle
                ModelState.Remove("Region");
                ModelState.Remove("Service");
                ModelState.Remove("Customer");
                ModelState.Remove("PhoneNumber");
                ModelState.Remove("ServiceType");
                ModelState.Remove("CustomerName");
                ModelState.Remove("EmployeeName");
                ModelState.Remove("Employee");

                if (id != employee.Id)
                {
                    TempData["FormErrors"] = "Geçersiz çalışan ID'si.";
                    await LoadViewBagData();
                    return View(employee);
                }

                if (string.IsNullOrEmpty(employee.FullName))
                {
                    TempData["FormErrors"] = "Ad Soyad alanı boş olamaz.";
                    await LoadViewBagData();
                    return View(employee);
                }
                
                if (string.IsNullOrEmpty(employee.Email))
                {
                    TempData["FormErrors"] = "E-posta alanı boş olamaz.";
                    await LoadViewBagData();
                    return View(employee);
                }
                
                if (string.IsNullOrEmpty(employee.PhoneNumber))
                {
                    TempData["FormErrors"] = "Telefon alanı boş olamaz.";
                    await LoadViewBagData();
                    return View(employee);
                }
                
                if (string.IsNullOrEmpty(employee.Specialization))
                {
                    TempData["FormErrors"] = "Uzmanlık alanı boş olamaz.";
                    await LoadViewBagData();
                    return View(employee);
                }

                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    TempData["FormErrors"] = "Çalışan bulunamadı.";
                    await LoadViewBagData();
                    return View(employee);
                }

                // E-posta değişmişse ve başka bir kullanıcı tarafından kullanılıyorsa kontrol et
                if (user.Email != employee.Email)
                {
                    var existingUser = await _userManager.FindByEmailAsync(employee.Email);
                    if (existingUser != null && existingUser.Id != user.Id)
                    {
                        TempData["FormErrors"] = "Bu e-posta adresi başka bir kullanıcı tarafından kullanılıyor.";
                        await LoadViewBagData();
                        return View(employee);
                    }
                }

                // Kullanıcı bilgilerini güncelle
                user.FullName = employee.FullName;
                user.Email = employee.Email;
                user.UserName = employee.Email;
                user.PhoneNumber = employee.PhoneNumber;
                user.Specialization = employee.Specialization;
                user.UpdatedAt = DateTime.UtcNow;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    // Employee tablosunu güncelle
                    var dbEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == id);
                    if (dbEmployee != null)
                    {
                        dbEmployee.Expertise = employee.Specialization;
                        dbEmployee.UpdatedAt = DateTime.UtcNow;
                        await _context.SaveChangesAsync();
                    }

                    TempData["SuccessMessage"] = "Çalışan başarıyla güncellendi.";
                    return RedirectToAction(nameof(Employees));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                await LoadViewBagData();
                return View(employee);
            }
            catch (Exception ex)
            {
                TempData["FormErrors"] = "Çalışan güncellenirken bir hata oluştu: " + ex.Message;
                await LoadViewBagData();
                return View(employee);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployee(string id)
        {
            var employee = await _userManager.FindByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(employee);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Employees));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return RedirectToAction(nameof(Employees));
        }

        [HttpGet]
        public async Task<IActionResult> ViewClientDetails(string id)
        {
            var client = await _userManager.FindByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            var appointments = await _context.Appointments
                .Include(a => a.Customer)
                .Include(a => a.Service)
                .Include(a => a.Employee)
                .Where(a => a.CustomerId == id)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            var viewModel = new ClientDetailsViewModel
            {
                Client = client,
                Appointments = appointments
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteClient(string id)
        {
            var client = await _userManager.FindByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(client);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Clients));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return RedirectToAction(nameof(Clients));
        }

        [HttpGet]
        public IActionResult AddService()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddService(Service service)
        {
            if (ModelState.IsValid)
            {
                service.IsActive = true;
                _context.Services.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Services));
            }
            return View(service);
        }

        [HttpGet]
        public async Task<IActionResult> Services()
        {
            ViewBag.Notifications = new List<string> { "Yeni randevu talebi geldi.", "Bir çalışan profilini güncelledi.", "Sistem yedeği başarıyla alındı." };
            ViewBag.Messages = new List<string> { "Hoş geldiniz, yönetici!", "Yeni bir mesajınız var.", "Destek ekibiyle iletişime geçin." };

            var services = await _context.Services
                .Include(s => s.SubServices)
                .Where(s => s.ParentServiceId == null)
                .ToListAsync();

            return View(services);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await _context.Services
                .Include(s => s.SubServices)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (service == null)
            {
                return NotFound();
            }

            // Alt hizmetleri de sil
            if (service.SubServices != null)
            {
                _context.Services.RemoveRange(service.SubServices);
            }

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Services));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleServiceStatus(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            service.IsActive = !service.IsActive;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Services));
        }

        [HttpGet]
        public async Task<IActionResult> EditService(int id)
        {
            var service = await _context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        [HttpPost]
        public async Task<IActionResult> EditService(Service service)
        {
            if (ModelState.IsValid)
            {
                _context.Update(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Services));
            }
            return View(service);
        }

        [HttpPost]
        public async Task<IActionResult> AddSubService(Service service)
        {
            if (ModelState.IsValid)
            {
                service.IsActive = true;
                _context.Services.Add(service);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Services));
            }
            var services = await _context.Services.Include(s => s.SubServices).ToListAsync();
            return View("Services", services);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Employees = new SelectList(await _context.Employees
                .Include(e => e.User)
                .ToListAsync(), "Id", "User.FullName");
            
            ViewBag.Services = new SelectList(await _context.Services
                .Where(s => s.ParentServiceId != null)
                .ToListAsync(), "Id", "Name");

            ViewBag.Customers = new SelectList(await _userManager.GetUsersInRoleAsync("Client"), "Id", "FullName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,EmployeeId,ServiceId,AppointmentDate,Description")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                appointment.Status = AppointmentStatus.Pending;
                appointment.CreatedAt = DateTime.UtcNow;

                _context.Add(appointment);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Randevu başarıyla oluşturuldu.";
                return RedirectToAction(nameof(Appointments));
            }

            // Hata durumunda dropdown listeleri tekrar yükle
            ViewBag.Employees = new SelectList(await _context.Employees
                .Include(e => e.User)
                .ToListAsync(), "Id", "User.FullName");
            
            ViewBag.Services = new SelectList(await _context.Services
                .Where(s => s.ParentServiceId != null)
                .ToListAsync(), "Id", "Name");

            ViewBag.Customers = new SelectList(await _userManager.GetUsersInRoleAsync("Client"), "Id", "FullName");

            return View(appointment);
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
} 