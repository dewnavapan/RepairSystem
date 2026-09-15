using Microsoft.AspNetCore.Identity;

namespace RepairSystem.Web.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
