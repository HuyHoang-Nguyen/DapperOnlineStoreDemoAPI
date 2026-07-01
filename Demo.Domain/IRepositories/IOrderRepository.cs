using Demo.Domain.Entities;
using Demo.Domain.Enum;
using Demo.Domain.Models;

namespace Demo.Domain.IRepositories
{
    public interface IOrderRepository
    {
        Task<Order> CreateOrderAsync(Guid userId, IEnumerable<CartItemsModel> cartItems, string? couponCode, decimal discountAmount);
        Task<IEnumerable<Order>> GetOrdersAsync(Guid userId);
        Task<Order?> GetByIdAsync(Guid orderId, Guid userId);
        Task<IEnumerable<OrderItem>> GetOrderItemsAsync(Guid orderId);
        Task DeleteOrderAsync(Guid orderId, Guid userId);
        Task<bool> CodeExistsAsync(string code);
        Task UpdateOrderStatusAsync(Guid orderId, EnumOrderStatus status);
    }
}
