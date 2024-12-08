using AspNetCore_WebAPP_MVC_PROJE.Models.DbSets;

namespace AspNetCore_WebAPP_MVC_PROJE.Models.MVVM
{
    public class MainPageModel
    {
        public List<Product>? NewProducts { get; set; }
        public List<Product>? SpecialProducts { get; set; }
        public List<Product>? DiscountProducts { get; set; }
        public List<Product>? HighlightedProducts { get; set; }
        public List<Product>? TopSellerProducts { get; set; }
        public List<Product>? SliderProducts { get; set; }
        public List<Product>? StarredProducts { get; set; }
        public List<Product>? OpportunityProducts { get; set; }
        public List<Product>? AttentionedProducts { get; set; }
        public Product? ProductOfTheDay { get; set; }
        public Product DetailsOfProduct { get; set; }
        public string? CategoryName { get; set; }
        public int CategoryID { get; set; }
        public string BrandName { get; set; }
        public List<Product> RelatedProducts { get; set; }
    }
}
