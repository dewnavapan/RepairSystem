using System.ComponentModel.DataAnnotations;
namespace RepairSystem.Web.Models.Entities
{
    public class Notification
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        [Required]
        [MaxLength(255)]
        public string Message { get; set; } = string.Empty;

        public int? RelatedRequestId { get; set; }
        public bool IsRead { get; set; } = false;

        public DateTime CreateOn { get; set; } = DateTime.UtcNow;
    }
}
