using Microsoft.AspNetCore.Identity;
using RepairSystem.Web.Models.Entities;

namespace RepairSystem.Web.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndDefaultUsersAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // 1. สร้าง Roles มาตรฐาน
            string[] roleNames = { "Admin", "Technician", "Requester" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. สร้างบัญชีช่างซ่อม (Technician Default)
            var techEmail = "tech@repairsystem.com";
            var techUser = await userManager.FindByEmailAsync(techEmail);
            if (techUser == null)
            {
                var newTech = new ApplicationUser
                {
                    UserName = techEmail,
                    Email = techEmail,
                    FullName = "นายช่าง ประจำระบบ",
                    EmailConfirmed = true // ข้ามการยืนยันอีเมลไปเลย
                };

                // รหัสผ่านต้องมีพิมพ์เล็ก พิมพ์ใหญ่ ตัวเลข และอักขระพิเศษ ตามมาตรฐาน Identity
                var createTechUser = await userManager.CreateAsync(newTech, "Password123!");
                if (createTechUser.Succeeded)
                {
                    // กำหนดสิทธิ์ให้บัญชีนี้เป็น Technician
                    await userManager.AddToRoleAsync(newTech, "Technician");
                }
            }
        }
    }
}