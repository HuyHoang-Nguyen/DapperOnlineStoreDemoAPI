using DapperOnlineStoreAPI.Models;

namespace DapperOnlineStoreAPI.Services.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<CartItemsModel>> GetCart(Guid userId);
        Task AddToCart(Guid userId, Guid productId, int quantity);
        Task UpdateCartItem(Guid userId, Guid productId, int quantity);
        Task RemoveCartItem(Guid userId, Guid productId);
    }
}
