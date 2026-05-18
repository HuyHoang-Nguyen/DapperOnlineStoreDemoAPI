namespace DapperOnlineStoreAPI.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
