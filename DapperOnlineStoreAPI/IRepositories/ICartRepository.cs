using DapperOnlineStoreAPI.Models;

namespace DapperOnlineStoreAPI.IRepositories
{
    public interface ICartRepository
    {
        Task AddToCart(Guid productId, int quantity);
        Task<IEnumerable<CartItemsModel>> GetCart();
    }
}
