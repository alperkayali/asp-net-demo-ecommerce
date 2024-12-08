using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCore_WebAPP_MVC_PROJE.Models.DbSets
{
    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }

        [StringLength(100)]
        [Required]
        public string? ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int CategoryID { get; set; }
        public int SupplierID { get; set; }

        private int _Stock { get; set; }
        public int Stock
        {
            get { return _Stock; }
            set { _Stock = Math.Abs(value); }
        }

        private int _Discount { get; set; }
        public int Discount
        {
            get { return _Discount; }
            set { _Discount = Math.Abs(value); }
        }

        public int StatusID { get; set; }
        public DateTime AddDate { get; set; }
        public string? Keywords { get; set; }
        private int _Kdv { get; set; }
        public int Kdv
        {
            get { return _Kdv; }
            set { _Kdv = Math.Abs(value); }
        }
        private int _HighLighted { get; set; }
        public int HighLighted
        {
            get { return _HighLighted; }
            set { _HighLighted = Math.Abs(value); }
        }
        private int _TopSeller { get; set; }
        public int TopSeller
        {
            get { return _TopSeller; }
            set { _TopSeller = Math.Abs(value); }
        }

        private int _Related { get; set; }
        public int Related
        {
            get { return _Related; }
            set { _Related = Math.Abs(value); }
        }

        public string? Notes { get; set; }

        public string? PhotoPath { get; set; }
        public bool Active { get; set; }
        public string? SliderPhotoPath { get; set; }
    }
}
