namespace DapperOnlineStoreAPI.Models
{
    public class AddToCartModel
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
