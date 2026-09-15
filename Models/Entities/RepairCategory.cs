using System.ComponentModel.DataAnnotations;
namespace RepairSystem.Web.Models.Entities
{
    public class RepairCategory
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
