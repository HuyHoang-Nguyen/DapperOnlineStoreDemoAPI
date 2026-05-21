using DapperOnlineStoreAPI.Entities;

namespace DapperOnlineStoreAPI.Services.Interfaces
{
    public interface IOrderService
    {
        Task<Guid> OrderCheckoutAsync(Guid userId, string? couponCode);
        Task<IEnumerable<Order>> GetOrdersAsync(Guid userId);
        Task<Order?> GetByIdAsync(Guid orderId, Guid userId);
        Task<IEnumerable<OrderItem>> GetOrderItemsAsync(Guid orderId);
        Task DeleteOrderAsync(Guid orderId, Guid userId);
    }
}
