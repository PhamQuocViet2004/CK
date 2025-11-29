using System.Web.Mvc;

namespace project1.Controllers
{
    public class HomeController : Controller
    {
        // Trang chủ
        public ActionResult Index()
        {
            return View();
        }

        // Trang giới thiệu
        public ActionResult About()
        {
            return View();
        }
    }
}
