using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YourNamespace.Controllers
{
    public class AlbumController : Controller
    {
        // Đường dẫn thư mục chứa ảnh
        private string ImagePath = "~/Content/Images/";

        // Hiển thị danh sách ảnh
        public ActionResult Index()
        {
            string serverPath = Server.MapPath(ImagePath);
            if (!Directory.Exists(serverPath))
            {
                Directory.CreateDirectory(serverPath);
            }

            var files = Directory.GetFiles(serverPath)
                .Select(f => Path.GetFileName(f)) // chỉ lấy tên file
                .ToList();

            return View(files);
        }

        // Upload ảnh
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                string fileName = Path.GetFileName(file.FileName);
                string path = Path.Combine(Server.MapPath(ImagePath), fileName);

                file.SaveAs(path);
            }
            return RedirectToAction("Index");
        }

        // Xóa ảnh
        public ActionResult Delete(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                string path = Path.Combine(Server.MapPath(ImagePath), fileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
