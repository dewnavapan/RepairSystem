using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepairSystem.Web.Data;

namespace RepairSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // ดึงข้อมูลแจ้งซ่อม "ทั้งหมด" พร้อมผูกข้อมูลผู้แจ้ง ช่างซ่อม และหมวดหมู่
            var allRequests = await _context.RepairRequests
                .Include(r => r.Category)
                .Include(r => r.Requester)
                .Include(r => r.Technician)
                .OrderByDescending(r => r.CreatedOn)
                .ToListAsync();

            return View(allRequests);
        }
    }
}