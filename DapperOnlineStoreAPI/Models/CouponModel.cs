namespace DapperOnlineStoreAPI.Models
{
    public class CouponModel
    {
        public string? Code { get; set; }
        public string? DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
    }
}
