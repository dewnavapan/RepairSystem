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
        public DbSet<RepairRequest> RepairRequest { get; set; }
        public DbSet<RepairAttachment> RepairAttachments { get; set; }
        public DbSet<RepairStatusHistory> RepairStatusHistories { get; set; }
        public DbSet<Notification> Notification { get; set; }

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

        }

    }
}
