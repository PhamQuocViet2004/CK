using System.Web.Mvc;
using System.Web.Routing;

namespace Lab2
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Show Product",
                url: "San-Pham/Danh-Sach-San-Pham",
                defaults: new { controller = "Product", action = "ShowProduct" }
            );

            routes.MapRoute(
                name: "Product",
                url: "San-Pham",
                defaults: new { controller = "Product", action = "ShowProduct" }
            );

            routes.MapRoute(
                name: "Edit Product",
                url: "San-Pham/Sua/{productId}",
                defaults: new { controller = "Product", action = "EditProduct" },
                constraints: new { productId = @"\d{1,4}" }
            );

            routes.MapRoute(
                name: "Details Product",
                url: "San-Pham/{productName}/{productId}",
                defaults: new { controller = "Product", action = "DetailsProduct", productName = UrlParameter.Optional },
                constraints: new { productId = @"\d{1,4}" }
            );

            // Route mặc định
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
