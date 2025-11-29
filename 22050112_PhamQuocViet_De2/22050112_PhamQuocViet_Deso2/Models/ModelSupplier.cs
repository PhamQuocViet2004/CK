using System.ComponentModel.DataAnnotations;

namespace Deso2.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierID { get; set; }

        [Required, StringLength(100)]
        public string SupplierName { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }
    }
}
