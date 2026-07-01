using Demo.Domain.Entities;
using Demo.Domain.Enum.EnumError;
using Demo.Domain.GlobalExceptionHandler;
using Demo.Domain.IRepositories;
using Demo.Domain.Services.Interfaces;

namespace Demo.Domain.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly ICouponService _couponService;
        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, ICouponRepository couponRepository, ICouponService couponService)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _couponRepository = couponRepository;
            _couponService = couponService;
        }
        public async Task<Guid> OrderCheckoutAsync(Guid userId, string? couponCode)
        {
            if (userId == Guid.Empty)
            {
                throw new ValidationException(new List<string>
                {
                    EnumOrderValidationError.UserIdInvalid.ToString()
                });
            }
            var cartItems = (await _cartRepository.GetCart(userId)).ToList();
            if (!cartItems.Any())
            {
                throw new ValidationException(new List<string>
                {
                    EnumOrderValidationError.CartEmpty.ToString()
                });
            }

            decimal discountAmount = 0;
            if (!string.IsNullOrEmpty(couponCode))
            {
                var cartTotal = cartItems.Sum(x => (x.DiscountPrice ?? x.Price) * x.Quantity);
                var couponResult = await _couponService.ValidateAsync(couponCode, cartTotal, userId);
                discountAmount = couponResult.DiscountAmount;
            }

            var order = await _orderRepository.CreateOrderAsync(userId, cartItems, couponCode, discountAmount);
            if (!string.IsNullOrEmpty(couponCode))
            {
                var coupon = await _couponRepository.GetByCodeAsync(couponCode);
                if (coupon != null)
                {
                    await _couponRepository.DescUsageLimitAsync(coupon.Id);
                }
            }
            return order.Id;
        }
        public async Task<IEnumerable<Order>> GetOrdersAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ValidationException(new List<string>
                { 
                    EnumOrderValidationError.UserIdInvalid.ToString() 
                });
            }
            return await _orderRepository.GetOrdersAsync(userId);
        }
        public async Task<Order?> GetByIdAsync(Guid orderId, Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ValidationException(new List<string>
                {
                    EnumOrderValidationError.UserIdInvalid.ToString()
                });
            }
            if (orderId == Guid.Empty)
            {
                throw new ValidationException(new List<string>
                {
                    EnumOrderValidationError.UserIdInvalid.ToString()
                });
            }
            var order = await _orderRepository.GetByIdAsync(orderId, userId);
            if (order == null)
            {
                throw new ValidationException(new List<string>()
                {
                    EnumOrderValidationError.OrderNotfound.ToString()
                });
            }
            return order;
        }
        public async Task<IEnumerable<OrderItem>> GetOrderItemsAsync(Guid orderId)
        {
            if (orderId == Guid.Empty)
            {
                throw new ValidationException(new List<string>
                {
                    EnumOrderValidationError.OrderIdInvalid.ToString()
                });
            }
            return await _orderRepository.GetOrderItemsAsync(orderId);
        }
        public async Task DeleteOrderAsync(Guid orderId, Guid userId)
        {
            if (orderId == Guid.Empty)
            {
                throw new ValidationException(new List<string>
                {
                    EnumOrderValidationError.OrderIdInvalid.ToString()
                });
            }
            var order = await _orderRepository.GetByIdAsync(orderId, userId);
            if (order == null)
            {
                throw new ValidationException(new List<string>
                {
                    EnumOrderValidationError.OrderNotfound.ToString()
                });
            }
            await _orderRepository.DeleteOrderAsync(orderId, userId);
        }
    }
}
