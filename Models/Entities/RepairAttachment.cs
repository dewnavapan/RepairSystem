using System.ComponentModel.DataAnnotations;
namespace RepairSystem.Web.Models.Entities
{
    public class RepairAttachment
    {
        public int Id { get; set; }
        public int RepairRequestId { get; set; }
        public RepairRequest? RepairRequest { get; set; }

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string FilePath { get; set; } = string.Empty;

        public DateTime UploadOn { get; set; }  = DateTime.UtcNow;
    }
}
