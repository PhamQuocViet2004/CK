using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CK.Data;
using CK.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CK.Controllers
{
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Jobs
        public async Task<IActionResult> Index(string searchString)
        {
            // 1. Tạo câu truy vấn cơ bản (Chưa sắp xếp vội)
            var jobs = from j in _context.Jobs
                       where j.IsActive == true
                       select j;

            // 2. Nếu có từ khóa tìm kiếm thì lọc danh sách
            if (!String.IsNullOrEmpty(searchString))
            {
                jobs = jobs.Where(s => s.Title.Contains(searchString));
            }

            // 3. Cuối cùng mới Sắp xếp (Tin mới nhất lên đầu) và chuyển thành List
            var finalModel = await jobs.OrderByDescending(j => j.CreatedDate).ToListAsync();

            return View(finalModel);
        }

        // --- BỔ SUNG QUAN TRỌNG: Xem lịch sử nộp đơn ---
        [Authorize] // Phải đăng nhập mới xem được
        public async Task<IActionResult> MyApplications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var myApps = await _context.JobApplications
                .Include(a => a.Job) // Kèm thông tin Job
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.AppliedDate)
                .ToListAsync();

            return View(myApps);
        }
        // -----------------------------------------------

        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var job = await _context.Jobs.FirstOrDefaultAsync(m => m.Id == id);
            if (job == null) return NotFound();

            return View(job);
        }

        // --- KHU VỰC ADMIN (Thêm [Authorize(Roles = "Admin")]) ---

        // GET: Jobs/Create
        [Authorize(Roles = "Admin")] // Chỉ Admin được vào
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jobs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Chỉ Admin được tạo
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Requirements,Salary,Location,CreatedDate,IsActive")] Job job)
        {
            if (ModelState.IsValid)
            {
                _context.Add(job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(job);
        }

        // GET: Jobs/Edit/5
        [Authorize(Roles = "Admin")] // Chỉ Admin được sửa
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var job = await _context.Jobs.FindAsync(id);
            if (job == null) return NotFound();
            return View(job);
        }

        // POST: Jobs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Chỉ Admin được lưu sửa
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Requirements,Salary,Location,CreatedDate,IsActive")] Job job)
        {
            if (id != job.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(job.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(job);
        }

        // GET: Jobs/Delete/5
        [Authorize(Roles = "Admin")] // Chỉ Admin được xóa
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var job = await _context.Jobs.FirstOrDefaultAsync(m => m.Id == id);
            if (job == null) return NotFound();

            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Chỉ Admin được xác nhận xóa
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job != null) _context.Jobs.Remove(job);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobExists(int id)
        {
            return _context.Jobs.Any(e => e.Id == id);
        }

        // --- KHU VỰC ỨNG VIÊN NỘP HỒ SƠ ---
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Apply(int jobId, IFormFile cvFile, string coverLetter)
        {
            try
            {
                if (cvFile == null || cvFile.Length == 0)
                {
                    TempData["Error"] = "Vui lòng chọn file CV.";
                    return RedirectToAction("Details", new { id = jobId });
                }

                string fileName = "cv_" + Guid.NewGuid().ToString() + "_" + cvFile.FileName;
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "cvs", fileName);

                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    await cvFile.CopyToAsync(stream);
                }

                var application = new JobApplication
                {
                    JobId = jobId,
                    UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    CVPatch = fileName,
                    CoverLetter = coverLetter,
                    AppliedDate = DateTime.Now,
                    Status = "Pending"
                };

                _context.JobApplications.Add(application);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Nộp hồ sơ thành công! Nhà tuyển dụng sẽ sớm liên hệ.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("Details", new { id = jobId });
            }
        }
    }
}