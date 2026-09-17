using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RepairSystem.Web.Data;
using RepairSystem.Web.Models.Entities;
using RepairSystem.Web.Models.ViewModels;
using System.Security.Claims;

namespace RepairSystem.Web.Controllers
{
    // บังคับว่าต้องล็อกอิน และต้องมี Role เป็น Technician เท่านั้น
    [Authorize(Roles = "Technician")]
    public class TechnicianController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TechnicianController(ApplicationDbContext context)
        {
            _context = context;
        }

        // หน้า Dashboard ของช่าง
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // 1. ดึงงานกองกลาง (ยังไม่มีใครรับ)
            var availableJobs = await _context.RepairRequests
                .Include(r => r.Category)
                .Where(r => r.Status == "Submitted" && r.TechnicianId == null)
                .OrderBy(r => r.CreatedOn)
                .ToListAsync();

            // 2. ดึงงานที่ช่างคนนี้กำลังทำอยู่
            var myActiveJobs = await _context.RepairRequests
                .Include(r => r.Category)
                .Where(r => r.TechnicianId == userId && (r.Status == "Accepted" || r.Status == "InProgress"))
                .OrderByDescending(r => r.UpdateOn ?? r.CreatedOn)
                .ToListAsync();

            // ส่งข้อมูล 2 ชุดไปที่ View ผ่าน ViewBag (สำหรับโปรเจกต์ขนาดเล็กวิธีนี้สะดวกดีครับ)
            ViewBag.AvailableJobs = availableJobs;
            ViewBag.MyActiveJobs = myActiveJobs;

            return View();
        }

        // ฟังก์ชันสำหรับกดรับงาน
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClaimJob(int id)
        {
            // ค้นหางานจาก ID
            var repairRequest = await _context.RepairRequests
                .Include(r => r.StatusHistories)
                .FirstOrDefaultAsync(r => r.Id == id);

            // ป้องกันกรณีไม่เจองาน หรือมีช่างคนอื่นตัดหน้ารับไปแล้ว
            if (repairRequest == null || repairRequest.Status != "Submitted")
            {
                TempData["ErrorMessage"] = "งานนี้ถูกรับไปแล้ว หรือไม่พบข้อมูล";
                return RedirectToAction(nameof(Index));
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            // อัปเดตข้อมูลรับงาน
            repairRequest.TechnicianId = userId;
            repairRequest.Status = "Accepted";
            repairRequest.UpdateOn = DateTime.UtcNow;

            // บันทึกประวัติ
            repairRequest.StatusHistories.Add(new RepairStatusHistory
            {
                NewStatus = "Accepted",
                ChangedById = userId,
                Comment = "ช่างซ่อมกดรับงาน"
            });

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "รับงานสำเร็จ! กรุณาตรวจสอบในหัวข้อ 'งานที่ฉันรับผิดชอบ'";
            return RedirectToAction(nameof(Index));
        }

        // หน้าจอสำหรับอัปเดตงาน
        [HttpGet]
        public async Task<IActionResult> UpdateJob(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // ค้นหางาน และต้องแน่ใจว่าเป็นงานของช่างคนนี้เท่านั้น
            var repairRequest = await _context.RepairRequests
                .Include(r => r.Category)
                .FirstOrDefaultAsync(r => r.Id == id && r.TechnicianId == userId);

            if (repairRequest == null) return NotFound(); // ไม่เจองาน หรือไม่มีสิทธิ์

            var vm = new UpdateJobVM
            {
                Id = repairRequest.Id,
                Title = repairRequest.Title,
                Description = repairRequest.Description,
                Location = repairRequest.Location,
                CategoryName = repairRequest.Category?.Name ?? "ไม่ระบุ",
                CurrentStatus = repairRequest.Status,
                StatusOptions = GetStatusOptions(repairRequest.Status)
            };

            return View(vm);
        }

        // รับข้อมูลที่ช่างกรอกมาเพื่อบันทึกลง Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateJob(UpdateJobVM vm)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var repairRequest = await _context.RepairRequests
                .Include(r => r.StatusHistories)
                .FirstOrDefaultAsync(r => r.Id == vm.Id && r.TechnicianId == userId);

            if (repairRequest == null) return NotFound();

            if (ModelState.IsValid)
            {
                string oldStatus = repairRequest.Status;
                repairRequest.Status = vm.NewStatus;
                repairRequest.UpdateOn = DateTime.UtcNow;

                // บันทึกประวัติพร้อมข้อความที่ช่างกรอก
                repairRequest.StatusHistories.Add(new RepairStatusHistory
                {
                    OldStatus = oldStatus,
                    NewStatus = vm.NewStatus,
                    ChangedById = userId,
                    Comment = vm.Comment
                });

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "อัปเดตสถานะงานเรียบร้อยแล้ว";
                return RedirectToAction(nameof(Index));
            }

            vm.StatusOptions = GetStatusOptions(repairRequest.Status);
            return View(vm);
        }

        // Helper Method กำหนดสถานะที่เลือกได้ตาม State Machine
        private List<SelectListItem> GetStatusOptions(string currentStatus)
        {
            var options = new List<SelectListItem>();
            if (currentStatus == "Accepted")
            {
                options.Add(new SelectListItem { Value = "InProgress", Text = "กำลังดำเนินการซ่อม (In Progress)" });
            }
            if (currentStatus == "Accepted" || currentStatus == "InProgress")
            {
                options.Add(new SelectListItem { Value = "Completed", Text = "ซ่อมเสร็จสิ้น (Completed)" });
            }
            return options;
        }
    }
}