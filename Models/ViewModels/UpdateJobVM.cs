using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RepairSystem.Web.Models.ViewModels
{
    public class UpdateJobVM
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string CurrentStatus { get; set; } = string.Empty;

        [Required(ErrorMessage = "กรุณาเลือกสถานะใหม่")]
        [Display(Name = "อัปเดตสถานะเป็น")]
        public string NewStatus { get; set; } = string.Empty;

        [Display(Name = "บันทึกการซ่อม / หมายเหตุ")]
        public string? Comment { get; set; }

        public IEnumerable<SelectListItem>? StatusOptions { get; set; }
    }
}