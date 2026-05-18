using DapperOnlineStoreAPI.Entities;
using DapperOnlineStoreAPI.Models;

namespace DapperOnlineStoreAPI.IRepositories
{
    public interface IOrderRepository
    {
        Task<Guid> CreateOrderAsync(Guid userId, IEnumerable<CartItemsModel> cartItems);
        Task<IEnumerable<Order>> GetOrdersAsync(Guid userId);
        Task<Order?> GetByIdAsync(Guid orderId, Guid userId);
        Task<IEnumerable<OrderItem>> GetOrderItemsAsync(Guid orderId);
        Task DeleteOrderAsync(Guid orderId, Guid userId);
    }
}
