using DapperOnlineStoreAPI.Entities;
using DapperOnlineStoreAPI.Enum.EnumError;
using DapperOnlineStoreAPI.GlobalExceptionHandler;
using DapperOnlineStoreAPI.IRepositories;
using DapperOnlineStoreAPI.Services.Interfaces;

namespace DapperOnlineStoreAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }
        public async Task<Guid> OrderCheckoutAsync(Guid userId)
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
            return await _orderRepository.CreateOrderAsync(userId, cartItems);
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
