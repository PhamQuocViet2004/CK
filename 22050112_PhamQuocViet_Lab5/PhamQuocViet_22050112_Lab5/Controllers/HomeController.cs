using System.Web.Mvc;

namespace Lab5.Controllers
{
    public class HomeController : Controller
    {
        // Trang chủ
        public ActionResult Index()
        {
            return View();
        }

        // Trang giới thiệu (tùy chọn)
        public ActionResult About()
        {
            ViewBag.Message = "Trang giới thiệu ứng dụng IT Library.";
            return View();
        }

        // Trang liên hệ (tùy chọn)
        public ActionResult Contact()
        {
            ViewBag.Message = "Liên hệ quản trị viên qua email: admin@itlibrary.com";
            return View();
        }
    }
}
