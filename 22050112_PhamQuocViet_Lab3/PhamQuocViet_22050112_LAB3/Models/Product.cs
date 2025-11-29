using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace YourNamespace.Models
{
    public class Product
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public decimal Price { get; set; }

        private static string filePath = HttpContext.Current.Server.MapPath("~/App_Data/Products.xml");

        // Lấy danh sách sản phẩm từ XML
        public static List<Product> GetAll()
        {
            XDocument doc = XDocument.Load(filePath);
            return doc.Descendants("Product")
                .Select(x => new Product
                {
                    ProductId = x.Element("ProductId").Value,
                    ProductName = x.Element("ProductName").Value,
                    Unit = x.Element("Unit").Value,
                    Price = decimal.Parse(x.Element("Price").Value)
                }).ToList();
        }

        // Thêm sản phẩm
        public static void Insert(Product p)
        {
            XDocument doc = XDocument.Load(filePath);
            XElement root = doc.Element("Products");
            root.Add(new XElement("Product",
                        new XElement("ProductId", p.ProductId),
                        new XElement("ProductName", p.ProductName),
                        new XElement("Unit", p.Unit),
                        new XElement("Price", p.Price)
                    ));
            doc.Save(filePath);
        }

        // Cập nhật sản phẩm
        public static void Update(Product p)
        {
            XDocument doc = XDocument.Load(filePath);
            var node = doc.Descendants("Product").FirstOrDefault(x => x.Element("ProductId").Value == p.ProductId);
            if (node != null)
            {
                node.Element("ProductName").Value = p.ProductName;
                node.Element("Unit").Value = p.Unit;
                node.Element("Price").Value = p.Price.ToString();
                doc.Save(filePath);
            }
        }

        // Xóa sản phẩm
        public static void Delete(string id)
        {
            XDocument doc = XDocument.Load(filePath);
            var node = doc.Descendants("Product").FirstOrDefault(x => x.Element("ProductId").Value == id);
            if (node != null)
            {
                node.Remove();
                doc.Save(filePath);
            }
        }
    }
}
