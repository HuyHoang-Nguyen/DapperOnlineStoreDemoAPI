using DapperOnlineStoreAPI.Models;

namespace DapperOnlineStoreAPI.IRepositories
{
    public interface ICartRepository
    {
        Task AddToCart(Guid userId, Guid productId, int quantity);
        Task<IEnumerable<CartItemsModel>> GetCart(Guid userId);
        Task UpdateCartItem(Guid userId, Guid productId, int quantity);
        Task RemoveCartItem(Guid userId, Guid productId);
    }
}
