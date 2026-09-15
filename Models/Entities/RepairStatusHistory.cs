using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace RepairSystem.Web.Models.Entities
{
    public class RepairStatusHistory
    {
        public int Id { get; set; }
        public int RepairRequestId { get; set; }
        public RepairRequest? RepairRequest { get; set; }

        [MaxLength(20)]
        public string? OldStatus { get; set; }

        [Required]
        [MaxLength(20)]
        public string? NewStatus { get; set; } = string.Empty;

        [Required]
        public string ChangedById { get; set; }

        [ForeignKey("ChangedById")]
        public ApplicationUser ChangedBy { get; set;}

        public string? Comment { get; set; }
        public DateTime CreateOn { get; set; } = DateTime.UtcNow;


    }
}
