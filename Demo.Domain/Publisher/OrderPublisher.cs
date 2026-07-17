using Demo.Domain.Models;
using Demo.Domain.Services.Interfaces;

namespace Demo.Domain.Publisher
{
    public class OrderPublisher
    {
        private readonly IQueueProvider _queue;
        public OrderPublisher(IQueueProvider queue)
        {
            _queue = queue;
        }
        public void PublishView(OrderRabbit model)
        {
            _queue.Publish("order_checkout_queue", new OrderRabbit()
            {
                UserId = model.UserId,
                CouponCode = model.CouponCode,
                CartItems = model.CartItems.ToList()
            });
        }
    }
    public class OrderRabbit
    {
        public Guid UserId { get; set; }
        public string? CouponCode { get; set; }
        public List<CartItemsModel> CartItems { get; set; } = new();
    }
}

