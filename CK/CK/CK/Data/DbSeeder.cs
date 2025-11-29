using CK.Models;
using Microsoft.AspNetCore.Identity;

namespace CK.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            // Lấy các dịch vụ cần thiết
            // Tìm 3 dòng này và thêm dấu chấm than (!) vào cuối cùng trước dấu chấm phẩy (;)

            var userManager = service.GetService<UserManager<IdentityUser>>()!; 
            var roleManager = service.GetService<RoleManager<IdentityRole>>()!; 
            var context = service.GetService<ApplicationDbContext>()!;        

            // 1. Tạo Role Admin và User nếu chưa có
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("User"));

            // 2. Tạo tài khoản Admin mẫu (admin@gmail.com / Admin@123)
            var adminUser = await userManager.FindByEmailAsync("admin@gmail.com");
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                };
                await userManager.CreateAsync(adminUser, "Admin@123");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // 3. Tạo dữ liệu Công việc mẫu (Nếu bảng Job trống)
            if (!context.Jobs.Any())
            {
                var jobs = new List<Job>
                {
                    new Job { Title = "Lập trình viên .NET", Salary = 20000000, Location = "Hồ Chí Minh", Description = "Phát triển web ASP.NET Core", Requirements = "Kinh nghiệm 1 năm", IsActive = true, CreatedDate = DateTime.Now },
                    new Job { Title = "Thực tập sinh Marketing", Salary = 5000000, Location = "Hà Nội", Description = "Hỗ trợ chạy quảng cáo FB", Requirements = "Sinh viên năm cuối", IsActive = true, CreatedDate = DateTime.Now },
                    new Job { Title = "Kế toán tổng hợp", Salary = 12000000, Location = "Đà Nẵng", Description = "Làm báo cáo thuế", Requirements = "Cẩn thận, tỉ mỉ", IsActive = true, CreatedDate = DateTime.Now },
                    new Job { Title = "Designer UI/UX", Salary = 18000000, Location = "Remote", Description = "Thiết kế App Mobile", Requirements = "Biết Figma", IsActive = true, CreatedDate = DateTime.Now.AddDays(-2) },
                    new Job { Title = "Trưởng phòng Kinh doanh", Salary = 30000000, Location = "Hồ Chí Minh", Description = "Quản lý đội sale 10 người", Requirements = "Kinh nghiệm 3 năm", IsActive = true, CreatedDate = DateTime.Now.AddDays(-5) },
                };
                context.Jobs.AddRange(jobs);
                await context.SaveChangesAsync();
            }
        }
    }
}