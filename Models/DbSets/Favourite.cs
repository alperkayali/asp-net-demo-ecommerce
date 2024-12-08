using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCore_WebAPP_MVC_PROJE.Models.DbSets
{
    public class Favourite
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FavouriteID { get; set; }
        public int ProductID { get; set; }
        public int UserID { get; set; }
        public string? ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Stock { get; set; }
        public string? PhotoPath { get; set; }
        public int KDV { get; set; }
        public bool ActiveFav { get; set; }
    }
}
