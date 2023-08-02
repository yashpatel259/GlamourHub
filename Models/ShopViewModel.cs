namespace GlamourHub.Models
{
    public class ShopViewModel
    {
        public PaginatedList<Product> Products { get; set; }
        public string CategoryFilter { get; set; }
        public string BrandFilter { get; set; }
        public string PriceFilter { get; set; }
        public int TotalCount { get; set; }
    }
}
