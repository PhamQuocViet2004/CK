using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CK.Data;
using CK.Models;
using Microsoft.AspNetCore.Authorization; // Thư viện bảo mật

namespace CK.Controllers
{
    [Authorize(Roles = "Admin")] // <--- QUAN TRỌNG: Khóa toàn bộ Controller này, chỉ Admin mới vào được
    public class ApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Applications
        public async Task<IActionResult> Index()
        {
            // Load danh sách hồ sơ kèm thông tin Job
            var applicationDbContext = _context.JobApplications.Include(j => j.Job);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Applications/Dashboard (Thống kê)
        public async Task<IActionResult> Dashboard()
        {
            var stats = new DashboardViewModel
            {
                JobCount = await _context.Jobs.CountAsync(),
                AppCount = await _context.JobApplications.CountAsync(),
                PendingCount = await _context.JobApplications.Where(x => x.Status == "Pending").CountAsync(),
                ApprovedCount = await _context.JobApplications.Where(x => x.Status == "Approved").CountAsync()
            };
            return View(stats);
        }

        // GET: Applications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var jobApplication = await _context.JobApplications
                .Include(j => j.Job)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobApplication == null) return NotFound();

            return View(jobApplication);
        }

        // GET: Applications/Create
        public IActionResult Create()
        {
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "Title");
            return View();
        }

        // POST: Applications/Create
        // SỬA ĐỔI QUAN TRỌNG: Thêm tham số IFormFile cvFile để nhận file upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,JobId,UserId,CoverLetter,Status,AppliedDate")] JobApplication jobApplication, IFormFile cvFile)
        {
            if (ModelState.IsValid)
            {
                // --- Xử lý lưu file CV ---
                if (cvFile != null && cvFile.Length > 0)
                {
                    // Tạo tên file duy nhất
                    string fileName = "cv_admin_upload_" + Guid.NewGuid().ToString() + "_" + cvFile.FileName;
                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "cvs", fileName);

                    using (var stream = new FileStream(uploadPath, FileMode.Create))
                    {
                        await cvFile.CopyToAsync(stream);
                    }

                    // Gán tên file vào Model
                    jobApplication.CVPatch = fileName;
                }
                // -------------------------

                _context.Add(jobApplication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "Title", jobApplication.JobId);
            return View(jobApplication);
        }

        // GET: Applications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var jobApplication = await _context.JobApplications.FindAsync(id);
            if (jobApplication == null) return NotFound();

            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "Title", jobApplication.JobId);
            return View(jobApplication);
        }

        // POST: Applications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JobId,UserId,CVPatch,CoverLetter,Status,AppliedDate")] JobApplication jobApplication)
        {
            if (id != jobApplication.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobApplicationExists(jobApplication.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["JobId"] = new SelectList(_context.Jobs, "Id", "Title", jobApplication.JobId);
            return View(jobApplication);
        }

        // GET: Applications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var jobApplication = await _context.JobApplications
                .Include(j => j.Job)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobApplication == null) return NotFound();

            return View(jobApplication);
        }

        // POST: Applications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobApplication = await _context.JobApplications.FindAsync(id);
            if (jobApplication != null)
            {
                // (Tuỳ chọn) Bạn có thể viết thêm code xóa file CV trong thư mục wwwroot/cvs tại đây nếu muốn sạch sẽ
                _context.JobApplications.Remove(jobApplication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobApplicationExists(int id)
        {
            return _context.JobApplications.Any(e => e.Id == id);
        }
    }
}