using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RepairSystem.Web.Data;
using RepairSystem.Web.Models.Entities;
using RepairSystem.Web.Models.ViewModels;

namespace RepairSystem.Web.Controllers
{
    [Authorize]
    public class RepairRequestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        // เพิ่ม IWebHostEnvironment เข้ามาใน Constructor
        public RepairRequestController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new CreateRepairRequestVM();
            await PopulateCategories(vm);
            return View(vm);
        }

        // --- ส่วนที่เพิ่มใหม่สำหรับรับข้อมูลจาก Form ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRepairRequestVM vm)
        {
            if (ModelState.IsValid)
            {
                // 1. ดึง ID ของคนที่กำลัง Login อยู่
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Unauthorized();

                // 2. สร้าง Entity หลัก
                var repairRequest = new RepairRequest
                {
                    Title = vm.Title,
                    Description = vm.Description,
                    Location = vm.Location,
                    CategoryId = vm.CategoryId,
                    RequesterId = userId,
                    Status = "Submitted"
                };

                // 3. จัดการไฟล์แนบ (ถ้ามี)
                if (vm.Attachments != null && vm.Attachments.Count > 0)
                {
                    if (vm.Attachments.Count > 3)
                    {
                        ModelState.AddModelError("Attachments", "แนบรูปภาพได้สูงสุด 3 รูปเท่านั้น");
                        await PopulateCategories(vm);
                        return View(vm);
                    }

                    // ตรวจสอบและสร้างโฟลเดอร์ uploads 
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

                    foreach (var file in vm.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                            if (!allowedExtensions.Contains(extension))
                            {
                                ModelState.AddModelError("Attachments", "ระบบรองรับเฉพาะไฟล์ .jpg และ .png เท่านั้น");
                                await PopulateCategories(vm);
                                return View(vm);
                            }

                            // สุ่มชื่อไฟล์ใหม่ป้องกันการทับกัน
                            string uniqueFileName = Guid.NewGuid().ToString() + extension;
                            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }

                            repairRequest.Attachments.Add(new RepairAttachment
                            {
                                FileName = file.FileName,
                                FilePath = uniqueFileName
                            });
                        }
                    }
                }

                // 4. บันทึกประวัติการเปลี่ยนสถานะเริ่มต้น
                repairRequest.StatusHistories.Add(new RepairStatusHistory
                {
                    NewStatus = "Submitted",
                    ChangedById = userId,
                    Comment = "สร้างรายการแจ้งซ่อมใหม่"
                });

                // 5. บันทึกลง Database
                _context.RepairRequests.Add(repairRequest);
                await _context.SaveChangesAsync();

                // บันทึกเสร็จแล้ว เด้งกลับไปหน้า Home ชั่วคราว (เดี๋ยวเราจะทำหน้ารายการทีหลัง)
                return RedirectToAction("Index", "Home");
            }

            // ถ้าข้อมูลไม่ถูกต้อง ให้ส่ง Categories กลับไปโหลดหน้าจอใหม่
            await PopulateCategories(vm);
            return View(vm);
        }

        // Helper Method สำหรับดึงข้อมูลหมวดหมู่ (ใช้ซ้ำได้)
        private async Task PopulateCategories(CreateRepairRequestVM vm)
        {
            vm.Categories = await _context.RepairCategories
                .Where(c => c.IsActive)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToListAsync();
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // ดึง ID ของ User ที่กำลัง Login
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            // ดึงข้อมูลแจ้งซ่อมเฉพาะของตัวเอง เรียงจากใหม่ไปเก่า
            var myRequests = await _context.RepairRequests
                .Include(r => r.Category) // Join เอาชื่อหมวดหมู่มาด้วย
                .Where(r => r.RequesterId == userId)
                .OrderByDescending(r => r.CreatedOn)
                .ToListAsync();

            return View(myRequests);
        }
    }
}