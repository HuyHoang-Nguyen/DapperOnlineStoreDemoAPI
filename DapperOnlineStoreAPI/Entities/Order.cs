using DapperOnlineStoreAPI.Enum;

namespace DapperOnlineStoreAPI.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public EnumOrderStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Code { get; set; }
        public string? CouponCode { get; set; }
        public decimal Subtotal { get; set; }
        public decimal? DiscountAmount { get; set; }

    }
}
