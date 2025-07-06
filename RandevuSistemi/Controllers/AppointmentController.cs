using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RandevuSistemi.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var appointments = await _context.Appointments
                .Include(a => a.AppointmentType)
                .Include(a => a.ServiceType)
                .Include(a => a.SubServiceType)
                .Include(a => a.Employee)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AppointmentDate)
                .ThenByDescending(a => a.AppointmentTime)
                .ToListAsync();

            return View(appointments);
        }

        public IActionResult Create()
        {
            // Randevu türlerini veritabanından al
            ViewBag.AppointmentTypes = new SelectList(_context.AppointmentTypes, "Id", "Name");

            // Ana hizmet türlerini veritabanından al (ParentServiceId null olanlar)
            ViewBag.ServiceTypes = new SelectList(_context.Services.Where(s => s.ParentServiceId == null), "Id", "Name");

            // Çalışanları veritabanından al
            ViewBag.Employees = new SelectList(_context.Employees.Where(e => e.IsActive), "Id", "Name");

            // Programlama dillerini veritabanından al
            var webSiteService = _context.Services.FirstOrDefault(s => s.Name == "Web Site Hizmeti");
            if (webSiteService != null)
            {
                ViewBag.ProgrammingLanguages = new SelectList(
                    _context.Services.Where(s => s.ParentServiceId == webSiteService.Id),
                    "Id",
                    "Name"
                );
            }
            else
            {
                ViewBag.ProgrammingLanguages = new SelectList(Enumerable.Empty<SelectListItem>());
            }

            return View();
        }

        [HttpGet]
        public IActionResult GetSubServiceTypes(int mainServiceId)
        {
            var subServices = _context.Services
                .Where(s => s.ParentServiceId == mainServiceId)
                .Select(s => new { id = s.Id, name = s.Name })
                .ToList();

            return Json(subServices);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Kullanıcının Client rolünde olup olmadığını kontrol et
            if (User.IsInRole("Client"))
            {
                return RedirectToAction(nameof(Details), new { id = id });
            }

            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            ViewBag.AppointmentTypes = new SelectList(_context.AppointmentTypes, "Id", "Name", appointment.AppointmentTypeId);
            ViewBag.ServiceTypes = new SelectList(_context.Services.Where(s => s.ParentServiceId == null), "Id", "Name", appointment.ServiceTypeId);
            ViewBag.Employees = new SelectList(_context.Employees.Where(e => e.IsActive), "Id", "Name", appointment.EmployeeId);

            return View(appointment);
        }

        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointments
                .Include(a => a.AppointmentType)
                .Include(a => a.ServiceType)
                .Include(a => a.SubServiceType)
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            // Sadece kullanıcının kendi randevusunu iptal edebilmesini sağla
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (appointment.UserId != userId)
            {
                return Forbid();
            }

            // Sadece beklemedeki randevular iptal edilebilir
            if (appointment.Status != "Beklemede")
            {
                return BadRequest("Sadece beklemedeki randevular iptal edilebilir.");
            }

            appointment.Status = "İptal Edildi";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
} 