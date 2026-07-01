namespace Demo.Domain.Models
{
    public class AddCartItemModel
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
