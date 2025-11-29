using Lab4.Models;
using System.Web.Mvc;

namespace Lab4.Controllers
{
    public class SurveyController : Controller
    {
        // Hiển thị form khảo sát
        public ActionResult Index()
        {
            return View();
        }

        // Nhận dữ liệu khi nhấn Submit
        [HttpPost]
        public ActionResult Index(Survey s)
        {
            if (ModelState.IsValid)
            {
                // Nếu hợp lệ, hiển thị kết quả
                return View("Result", s);
            }
            // Nếu có lỗi, hiển thị lại form
            return View(s);
        }
    }
}
