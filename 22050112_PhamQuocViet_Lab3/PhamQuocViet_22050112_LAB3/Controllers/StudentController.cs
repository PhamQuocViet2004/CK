using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YourNamespace.Data;
using YourNamespace.Models;

namespace YourNamespace.Controllers
{
    public class StudentController : Controller
    {
        private readonly string ImageFolder = "~/Content/StudentImages/";

        public ActionResult Index()
        {
            var list = StudentRepository.GetAll();
            return View(list);
        }

        // GET: Create
        public ActionResult Create()
        {
            return View(new Student());
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student model, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // xử lý file ảnh nếu có
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var ext = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                        if (!allowed.Contains(ext))
                        {
                            ModelState.AddModelError("", "Chỉ cho phép file ảnh .jpg .jpeg .png .gif");
                            return View(model);
                        }
                        if (imageFile.ContentLength > 2 * 1024 * 1024) // 2MB limit
                        {
                            ModelState.AddModelError("", "Kích thước file tối đa 2MB");
                            return View(model);
                        }

                        var serverFolder = Server.MapPath(ImageFolder);
                        if (!Directory.Exists(serverFolder)) Directory.CreateDirectory(serverFolder);

                        // tên file an toàn: StudentId sẽ được sinh nếu rỗng, nhưng để có tên file trước hết ta tạm tạo GUID
                        string newFileName = Guid.NewGuid().ToString("N") + ext;
                        var savePath = Path.Combine(serverFolder, newFileName);
                        imageFile.SaveAs(savePath);

                        model.ImageFileName = newFileName;
                    }

                    // nếu StudentId rỗng thì repository sẽ sinh
                    StudentRepository.Insert(model);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi: " + ex.Message);
                }
            }
            return View(model);
        }

        // GET: Edit
        public ActionResult Edit(string id)
        {
            var s = StudentRepository.GetById(id);
            if (s == null) return HttpNotFound();
            return View(s);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Student model, HttpPostedFileBase imageFile, string removeImage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existing = StudentRepository.GetById(model.StudentId);
                    if (existing == null) return HttpNotFound();

                    // nếu upload ảnh mới -> lưu và xóa ảnh cũ
                    if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        var ext = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                        var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        if (!allowed.Contains(ext))
                        {
                            ModelState.AddModelError("", "Chỉ cho phép file ảnh .jpg .jpeg .png .gif");
                            return View(model);
                        }
                        if (imageFile.ContentLength > 2 * 1024 * 1024)
                        {
                            ModelState.AddModelError("", "Kích thước file tối đa 2MB");
                            return View(model);
                        }

                        var serverFolder = Server.MapPath(ImageFolder);
                        if (!Directory.Exists(serverFolder)) Directory.CreateDirectory(serverFolder);

                        string newFileName = Guid.NewGuid().ToString("N") + ext;
                        var savePath = Path.Combine(serverFolder, newFileName);
                        imageFile.SaveAs(savePath);

                        // xóa ảnh cũ nếu có
                        if (!string.IsNullOrEmpty(existing.ImageFileName))
                        {
                            var old = Path.Combine(serverFolder, existing.ImageFileName);
                            if (System.IO.File.Exists(old)) System.IO.File.Delete(old);
                        }

                        model.ImageFileName = newFileName;
                    }
                    else if (!string.IsNullOrEmpty(removeImage) && removeImage == "on")
                    {
                        // xóa ảnh nếu người dùng tick remove
                        var serverFolder = Server.MapPath(ImageFolder);
                        if (!string.IsNullOrEmpty(existing.ImageFileName))
                        {
                            var old = Path.Combine(serverFolder, existing.ImageFileName);
                            if (System.IO.File.Exists(old)) System.IO.File.Delete(old);
                        }
                        model.ImageFileName = "";
                    }
                    else
                    {
                        // giữ ảnh cũ
                        model.ImageFileName = existing.ImageFileName;
                    }

                    StudentRepository.Update(model);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi: " + ex.Message);
                }
            }
            return View(model);
        }

        // GET: Delete
        public ActionResult Delete(string id)
        {
            var s = StudentRepository.GetById(id);
            if (s == null) return HttpNotFound();
            return View(s);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var s = StudentRepository.GetById(id);
            if (s == null) return HttpNotFound();

            // xóa ảnh
            var serverFolder = Server.MapPath(ImageFolder);
            if (!string.IsNullOrEmpty(s.ImageFileName))
            {
                var old = Path.Combine(serverFolder, s.ImageFileName);
                if (System.IO.File.Exists(old)) System.IO.File.Delete(old);
            }

            StudentRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
