using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCore_WebAPP_MVC_PROJE.Models.DbSets
{
    public class Supplier
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupplierID { get; set; }
        [StringLength(100)]
        [Required]
        public string? BrandName { get; set; }
        public string? PhotoPath { get; set; }
        public bool Active { get; set; }
    }
}
