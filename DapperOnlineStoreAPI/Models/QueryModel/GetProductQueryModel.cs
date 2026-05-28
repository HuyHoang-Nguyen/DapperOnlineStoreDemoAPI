namespace DapperOnlineStoreAPI.Models.QueryModel
{
    public class GetProductQueryModel
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? ImageUrl { get; set; }
        public decimal? Discount { get; set; }
        public DateTime? DiscountStart { get; set; }
        public DateTime? DiscountEnd { get; set; }
    }
}
