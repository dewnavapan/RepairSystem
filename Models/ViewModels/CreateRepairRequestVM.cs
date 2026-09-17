using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RepairSystem.Web.Models.ViewModels
{
    public class CreateRepairRequestVM
    {
        [Required(ErrorMessage = "กรุณาระบุหัวข้อปัญหา")]
        [MaxLength(100)]
        [Display(Name = "หัวข้อปัญหา")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "กรุณาระบุรายละเอียด")]
        [Display(Name = "รายละเอียดปัญหา")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "กรุณาระบุสถานที่")]
        [MaxLength(255)]
        [Display(Name = "สถานที่ (เช่น ตึก A ชั้น 2)")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "กรุณาเลือกประเภทงานซ่อม")]
        [Display(Name = "ประเภทงานซ่อม")]
        public int CategoryId { get; set; }

        // สำหรับแสดง Dropdown
        public IEnumerable<SelectListItem>? Categories { get; set; }

        // สำหรับแนบรูป (สูงสุด 3 รูป)
        [Display(Name = "แนบรูปภาพ (สูงสุด 3 รูป)")]
        public List<IFormFile>? Attachments { get; set; }
    }
}