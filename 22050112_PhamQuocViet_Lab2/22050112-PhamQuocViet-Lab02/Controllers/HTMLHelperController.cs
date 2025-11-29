using LAB2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LAB2.Controllers
{
    public class HTMLHelperController : Controller
    {
        // GET: HTMLHelper
        public ActionResult FormRegister()
        {
            // Tạo list cho DropDownList
            ViewBag.listCountry = new List<Country>
            {
                new Country { ID = "0",  Name = "-- Chọn Quốc Gia --" },
                new Country { ID = "VN", Name = "Việt Nam" },
                new Country { ID = "AT", Name = "Australia" },
                new Country { ID = "UK", Name = "Anh" },
                new Country { ID = "FR", Name = "Pháp" },
                new Country { ID = "US", Name = "Mỹ" },
                new Country { ID = "KP", Name = "Hàn Quốc" },
                new Country { ID = "HU", Name = "Hồng Kông" },
                new Country { ID = "CN", Name = "Trung Quốc" }
            };
            return View();
        }

        public ActionResult Register()
        {
            // lấy giá trị được  các trường đẩy lên server khi submit
            string fvr = "";

            TempData["UName"] = Request["txtUName"];
            TempData["Pass"] = Request["txtPass"];
            TempData["FName"] = Request["txtFName"];
            TempData["Gender"] = Request["Gender"];   
            TempData["Address"] = Request["txtAddress"];
            TempData["Email"] = Request["txtEmail"];
            TempData["Country"] = Request["Country"];

            if (!string.IsNullOrEmpty(Request["Reading"])) fvr += "Reading, ";
            if (!string.IsNullOrEmpty(Request["Shopping"])) fvr += "Shopping, ";
            if (!string.IsNullOrEmpty(Request["Sport"])) fvr += "Sport, ";

            TempData["Favourist"] = fvr.TrimEnd(' ', ',');

            return View();
        }
    }
}