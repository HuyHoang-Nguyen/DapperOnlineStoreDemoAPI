namespace DapperOnlineStoreAPI.Entities
{
    public class Coupon
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? DiscountType {  get; set; }
        public decimal DiscountValue { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int? UsageLimit { get; set; }
        public bool IsActive { get; set; }
        public int? MinOrderAmount { get; set; }
        public Guid? CategoryId { get; set; }
        public decimal? MinTotalAmount { get; set; }
    }
}
