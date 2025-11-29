using System.Linq;
using System.Web.Mvc;
using YourNamespace.Models;

namespace YourNamespace.Controllers
{
    public class ProductController : Controller
    {
        // Hiển thị danh sách
        public ActionResult Index()
        {
            var products = Product.GetAll();
            return View(products);
        }

        // Tạo mới
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product p)
        {
            if (ModelState.IsValid)
            {
                Product.Insert(p);
                return RedirectToAction("Index");
            }
            return View(p);
        }

        // Sửa
        public ActionResult Edit(string id)
        {
            var product = Product.GetAll().FirstOrDefault(x => x.ProductId == id);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product p)
        {
            if (ModelState.IsValid)
            {
                Product.Update(p);
                return RedirectToAction("Index");
            }
            return View(p);
        }

        // Xóa
        public ActionResult Delete(string id)
        {
            var product = Product.GetAll().FirstOrDefault(x => x.ProductId == id);
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            Product.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
