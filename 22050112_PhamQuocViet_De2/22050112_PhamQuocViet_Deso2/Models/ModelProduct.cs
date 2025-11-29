using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Deso2.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required, StringLength(150)]
        public string ProductName { get; set; }

        [Range(0, 1000000)]
        public decimal Unitprice { get; set; }

        public DateTime ExpiryDate { get; set; }

        [ForeignKey("Supplier")]
        public int SupplierID { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}
