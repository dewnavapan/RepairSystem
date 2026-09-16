using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;

namespace RepairSystem.Web.Models.Entities
{
    public class RepairRequest
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Location { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public RepairCategory? Category { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Submitted";

        [Required]
        public string RequesterId { get; set; } = string.Empty;
        [ForeignKey("RequesterId")]
        public ApplicationUser? Requester { get; set; }

        public string? TechnicianId { get; set; }
        [ForeignKey("TechnicianId")]
        public ApplicationUser? Technician { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateOn { get; set; }

        public ICollection<RepairAttachment> Attachments { get; set; } = new List<RepairAttachment>();
        public ICollection<RepairStatusHistory> StatusHistories { get; set; } = new List<RepairStatusHistory>();
    } 
}
