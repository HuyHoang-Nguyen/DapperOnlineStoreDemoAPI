using Demo.Domain.Publisher;
using Demo.Domain.Services;
using Demo.Domain.Services.Interfaces;

namespace Demo.Domain.Consumer
{
    public class OrderConsumer : QueueConsumer<OrderRabbit>
    {
        private readonly IOrderService _orderService;
        public OrderConsumer(IQueueProvider queueProvider, IOrderService orderService) : base(queueProvider)
        {
            _orderService = orderService;
        }
        protected override async Task Handle(OrderRabbit orderRabbit)
        {
            if (orderRabbit.UserId != null)
            {
                await _orderService.OrderCheckoutSnapshotAsync(orderRabbit.UserId, orderRabbit.CouponCode, orderRabbit.CartItems);
            }
        }
    }
}
