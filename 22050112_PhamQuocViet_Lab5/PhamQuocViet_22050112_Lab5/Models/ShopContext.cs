using System.Data.Entity;

namespace Lab5.Models
{
    public class ShopContext : DbContext
    {
        public ShopContext() : base("name=ShopContext")
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
