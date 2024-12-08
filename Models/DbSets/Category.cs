using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCore_WebAPP_MVC_PROJE.Models.DbSets
{
    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryID { get; set; }

        private int _ParentID { get; set; }
        public int ParentID
        {
            get { return _ParentID; }
            set { _ParentID = Math.Abs(value); }
        }

        [StringLength(100)]
        [Required]
        public string? CategoryName { get; set; }

        public bool Active { get; set; }
    }
}
