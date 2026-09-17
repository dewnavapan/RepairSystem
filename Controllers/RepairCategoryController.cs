using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepairSystem.Web.Data;
using RepairSystem.Web.Models.Entities;

namespace RepairSystem.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RepairCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RepairCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. หน้ารายการหมวดหมู่ทั้งหมด
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _context.RepairCategories.ToListAsync();
            return View(categories);
        }

        // 2. หน้าฟอร์มเพิ่มหมวดหมู่
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RepairCategory category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "เพิ่มหมวดหมู่สำเร็จ";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // 3. หน้าฟอร์มแก้ไขหมวดหมู่
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var category = await _context.RepairCategories.FindAsync(id);
            if (category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RepairCategory category)
        {
            if (id != category.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "แก้ไขหมวดหมู่สำเร็จ";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // 4. เปิด/ปิด การใช้งาน (Soft Delete)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var category = await _context.RepairCategories.FindAsync(id);
            if (category != null)
            {
                category.IsActive = !category.IsActive; // สลับสถานะ True/False
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = category.IsActive ? "เปิดใช้งานหมวดหมู่แล้ว" : "ระงับการใช้งานหมวดหมู่แล้ว";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}