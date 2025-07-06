using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using AppointmentSystem.Data;
using AppointmentSystem.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using AppointmentSystem.Models.ViewModels;
using AppointmentSystem.Services;

namespace AppointmentSystem.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotificationService _notificationService;

        public AppointmentController(
            ApplicationDbContext context, 
            IMemoryCache memoryCache,
            UserManager<ApplicationUser> userManager,
            INotificationService notificationService)
        {
            _context = context;
            _memoryCache = memoryCache;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        // GET: Appointment
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appointments = await _context.Appointments
                .Include(a => a.Service)
                .Include(a => a.Employee)
                    .ThenInclude(e => e.User)
                .Where(a => a.CustomerId == userId)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return View(appointments);
        }

        // GET: Appointment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appointment = await _context.Appointments
                .Include(a => a.Service)
                .Include(a => a.Employee)
                    .ThenInclude(e => e.User)
                .FirstOrDefaultAsync(a => a.Id == id && a.CustomerId == userId);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointment/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.MainServices = new SelectList(
                await _context.Services
                    .Where(s => s.ParentServiceId == null)
                    .ToListAsync(), 
                "Id", 
                "Name"
            );

            return View();
        }

        // GET: Appointment/GetSubServices
        [HttpGet]
        public async Task<IActionResult> GetSubServices(int mainServiceId)
        {
            var subServices = await _context.Services
                .Where(s => s.ParentServiceId == mainServiceId)
                .Select(s => new { value = s.Id, text = s.Name })
                .ToListAsync();

            return Json(subServices);
        }

        // POST: Appointment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.AppointmentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var appointment = new Appointment
                {
                    CustomerId = userId,
                    ServiceId = model.ServiceId,
                    EmployeeId = model.EmployeeId,
                    AppointmentDate = model.AppointmentDate,
                    Description = model.Description,
                    Status = AppointmentStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Add(appointment);
                await _context.SaveChangesAsync();

                // Bildirim oluştur
                await _notificationService.CreateAppointmentReminderAsync(appointment);

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Services = new SelectList(await _context.Services.ToListAsync(), "Id", "Name");
            ViewBag.Employees = new SelectList(await _context.Employees
                .Include(e => e.User)
                .ToListAsync(), "Id", "User.Name");

            return View(model);
        }

        // POST: Appointment/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == id && a.CustomerId == userId);

            if (appointment == null)
            {
                return NotFound();
            }

            if (appointment.Status != AppointmentStatus.Pending)
            {
                return BadRequest("Sadece bekleyen randevular iptal edilebilir.");
            }

            appointment.Status = AppointmentStatus.Cancelled;
            appointment.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Appointment/GetEmployeesByService
        [HttpGet]
        public async Task<IActionResult> GetEmployeesByService(int serviceId)
        {
            var employees = await _context.Employees
                .Include(e => e.User)
                .Where(e => e.IsActive)
                .Select(e => new { value = e.Id, text = e.User.FullName })
                .ToListAsync();

            return Json(employees);
        }

        public async Task<IActionResult> IndexAdmin()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            var isEmployee = await _userManager.IsInRoleAsync(user, "Employee");

            IQueryable<Appointment> appointmentsQuery = _context.Appointments
                .Include(a => a.Service)
                .Include(a => a.Employee)
                    .ThenInclude(e => e.User);

            if (!isAdmin)
            {
                if (isEmployee)
                {
                    var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == user.Id);
                    if (employee != null)
                    {
                        appointmentsQuery = appointmentsQuery.Where(a => a.EmployeeId == employee.Id);
                    }
                }
                else
                {
                    appointmentsQuery = appointmentsQuery.Where(a => a.CustomerId == user.Id);
                }
            }

            var appointments = await appointmentsQuery
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return View(appointments);
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Status = AppointmentStatus.Confirmed;
            appointment.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexAdmin));
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> EmployeeAppointments()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == user.Id);
            if (employee == null)
            {
                return NotFound();
            }

            var appointments = await _context.Appointments
                .Include(a => a.Service)
                .Include(a => a.Customer)
                .Where(a => a.EmployeeId == employee.Id)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return View(appointments);
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAppointmentStatus(int id, AppointmentStatus status)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == user.Id);
            
            if (employee == null || appointment.EmployeeId != employee.Id)
            {
                return Forbid();
            }

            appointment.Status = status;
            appointment.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(EmployeeAppointments));
        }

        [Authorize(Roles = "Employee")]
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

            var user = await _userManager.GetUserAsync(User);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == user.Id);
            
            if (employee == null || appointment.EmployeeId != employee.Id)
            {
                return Forbid();
            }

            // Müşteri listesini yükle
            var customers = await _userManager.GetUsersInRoleAsync("Client");
            ViewBag.Customers = new SelectList(customers, "Id", "FullName", appointment.CustomerId);

            // Hizmet listesini yükle
            var services = await _context.Services
                .Where(s => s.ParentServiceId != null && s.IsActive)
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

        [Authorize(Roles = "Employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAppointment(int id, [Bind("Id,CustomerId,EmployeeId,ServiceId,AppointmentDate,Description,Status,CreatedAt")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                TempData["FormErrors"] = "Geçersiz randevu ID'si.";
                return View(appointment);
            }

            var existingAppointment = await _context.Appointments.FindAsync(id);
            if (existingAppointment == null)
            {
                TempData["FormErrors"] = "Randevu bulunamadı.";
                return View(appointment);
            }

            var user = await _userManager.GetUserAsync(User);
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.UserId == user.Id);
            if (employee == null || existingAppointment.EmployeeId != employee.Id)
            {
                TempData["FormErrors"] = "Bu randevuyu güncelleme yetkiniz yok.";
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
                return RedirectToAction(nameof(EmployeeAppointments));
            }
            catch (Exception ex)
            {
                TempData["FormErrors"] = "Randevu güncellenirken bir hata oluştu: " + ex.Message;
                return View(appointment);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.Employee)
                .Include(a => a.Service)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            // Kullanıcı sadece kendi randevularını düzenleyebilir
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (appointment.CustomerId != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "Name");
            ViewBag.Services = new SelectList(await _context.Services.ToListAsync(), "Id", "Name");

            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,EmployeeId,ServiceId,AppointmentDate,Description,Status,CreatedAt")] Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return NotFound();
            }

            // Kullanıcı sadece kendi randevularını düzenleyebilir
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (appointment.CustomerId != userId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    appointment.UpdatedAt = DateTime.Now;
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "Name");
            ViewBag.Services = new SelectList(await _context.Services.ToListAsync(), "Id", "Name");

            return View(appointment);
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointments.Any(e => e.Id == id);
        }
    }
}