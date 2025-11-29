using System.Web.Mvc;
using LAB2.Models;

namespace LAB2.Controllers
{
    public class DangKyController : Controller
    {
        // GET: DangKy
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        // POST: DangKy
        [HttpPost]
        public ActionResult Index(DangKyThanhVien model)
        {
            if (ModelState.IsValid)
            {
                return View("Details", model);
            }
       
            return View(model);
        }

        public ActionResult Details(DangKyThanhVien model)
        {
            return View(model);
        }
    }
}
