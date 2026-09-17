using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RepairSystem.Web.Models.Entities;

namespace RepairSystem.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<RepairCategory> RepairCategories { get; set; }
        public DbSet<RepairRequest> RepairRequests { get; set; } 
        public DbSet<RepairAttachment> RepairAttachments { get; set; }
        public DbSet<RepairStatusHistory> RepairStatusHistories { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        string Test = "Dew";

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<RepairRequest>()
                .HasOne(x => x.Requester)
                .WithMany()
                .HasForeignKey(x => x.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RepairRequest>()
                .HasOne(x => x.Technician)
                .WithMany()
                .HasForeignKey(x => x.TechnicianId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RepairStatusHistory>()
                .HasOne(x => x.ChangedBy)
                .WithMany()
                .HasForeignKey(x => x.ChangedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<RepairCategory>().HasData(
            new RepairCategory { Id = 1, Name = "IT / อุปกรณ์คอมพิวเตอร์", IsActive = true },
            new RepairCategory { Id = 2, Name = "ไฟฟ้า", IsActive = true },
            new RepairCategory { Id = 3, Name = "ประปา", IsActive = true },
            new RepairCategory { Id = 4, Name = "ทั่วไป / อาคาร", IsActive = true },
            new RepairCategory { Id = 5, Name = "อื่นๆ", IsActive = true }
        );

        }

    }
}
