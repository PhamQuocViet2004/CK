using System.Web.Mvc;

namespace Deso2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Ứng dụng quản lý Supplier và Product - Deso2 MVC";
            return View();
        }
    }
}
