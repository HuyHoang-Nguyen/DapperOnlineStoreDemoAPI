using Demo.Domain.Publisher;
using Demo.Domain.Services;
using Demo.Domain.Services.Interfaces;

namespace Demo.Domain.Consumer
{
    public class OrderConsumer : QueueConsumer<OrderRabbit>
    {
        private readonly IOrderService _orderService;
        private readonly INotificationService _notificationService;
        public OrderConsumer(IQueueProvider queueProvider, IOrderService orderService, INotificationService notificationService) : base(queueProvider)
        {
            _orderService = orderService;
            _notificationService = notificationService;
        }
        protected override async Task Handle(OrderRabbit orderRabbit)
        {
            if (orderRabbit.UserId != null)
            {
                try
                {
                    await _orderService.OrderCheckoutSnapshotAsync(orderRabbit.UserId, orderRabbit.CouponCode, orderRabbit.CartItems);
                }
                catch (InvalidOperationException)
                {
                    await _notificationService.CreateAsync(orderRabbit.UserId, "Order failed - item ran out of stock", null);
                }
            }
        }
    }
}
