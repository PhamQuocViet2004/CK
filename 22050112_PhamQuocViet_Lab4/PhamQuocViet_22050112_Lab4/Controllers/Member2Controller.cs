using Lab4.Models;
using System.Web.Mvc;

namespace Lab4.Controllers
{
    public class Member2Controller : Controller
    {
        // GET: Trang đăng ký
        public ActionResult Register()
        {
            return View();
        }

        // POST: Nhận dữ liệu và hiển thị chi tiết
        [HttpPost]
        public ActionResult Register(Member2 m)
        {
            if (ModelState.IsValid)
            {
                // Dữ liệu hợp lệ → hiển thị trang chi tiết
                return View("Details2", m);
            }

            // Nếu có lỗi → hiển thị lại form kèm thông báo
            return View(m);
        }
    }
}
