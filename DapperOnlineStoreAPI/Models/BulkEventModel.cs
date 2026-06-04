namespace DapperOnlineStoreAPI.Models
{
    public class BulkEventModel
    {
        public List<Guid> ProductIds { get; set; } = new();
        public decimal EventDiscount { get; set; }
        public DateTime? EventStart { get; set; }
        public DateTime? EventEnd { get; set; }
    }
}
