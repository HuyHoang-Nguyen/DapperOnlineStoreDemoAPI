namespace DapperOnlineStoreAPI.Models
{
    public class CartItemsModel
    {
        public Guid ProductId { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public List<string>? ImageUrls { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal? Discount { get; set; }
        public decimal? DiscountPrice { get; set; }
    }
}
