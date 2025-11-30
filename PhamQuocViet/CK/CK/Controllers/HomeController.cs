using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CK.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CK.Data;
using Microsoft.EntityFrameworkCore;

namespace CK.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        // Inject các d?ch v? c?n thi?t vào ?ây
        public HomeController(ILogger<HomeController> logger,
                              RoleManager<IdentityRole> roleManager,
                              UserManager<IdentityUser> userManager,
                              ApplicationDbContext context)
        {
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        // L?y danh sách 6 vi?c làm m?i nh?t ?? hi?n th? trang ch?
        public async Task<IActionResult> Index()
        {
            var topJobs = await _context.Jobs
                                .Where(j => j.IsActive == true) // Ch? l?y tin ?ang hi?n
                                .OrderByDescending(j => j.CreatedDate) // Tin m?i nh?t lên ??u
                                .Take(6) // Ch? l?y 6 tin
                                .ToListAsync();
            return View(topJobs);
        }

        // --- Hàm t?o Admin (Ch? dùng ?? c?p quy?n l?n ??u) ---
        // Link ch?y: /Home/MakeMeAdmin
        public async Task<IActionResult> MakeMeAdmin()
        {
            // 1. T?o Role Admin n?u ch?a có
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // 2. L?y user ?ang ??ng nh?p hi?n t?i
            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                // 3. Thêm user vào nhóm Admin
                await _userManager.AddToRoleAsync(user, "Admin");

                // Tr? v? thông báo ti?ng Vi?t rõ ràng
                return Content($"? ?ã c?p quy?n Admin thành công cho tài kho?n: {user.UserName}. Vui lòng ??ng xu?t và ??ng nh?p l?i ?? c?p nh?t quy?n!");
            }

            return Content("? L?i: B?n ch?a ??ng nh?p. Vui lòng ??ng nh?p tr??c khi ch?y ???ng d?n này!");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}