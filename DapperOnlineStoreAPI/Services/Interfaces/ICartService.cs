using DapperOnlineStoreAPI.Models;

namespace DapperOnlineStoreAPI.Services.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<CartItemsModel>> GetCart();
        Task AddToCart(Guid productId, int quantity);
        Task UpdateCartItem(Guid productId, int quantity);
        Task RemoveCartItem(Guid productId);
    }
}
