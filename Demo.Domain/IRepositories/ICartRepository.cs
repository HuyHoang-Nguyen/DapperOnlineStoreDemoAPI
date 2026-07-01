using Demo.Domain.Models;

namespace Demo.Domain.IRepositories
{
    public interface ICartRepository
    {
        Task AddToCart(Guid userId, Guid productId, int quantity);
        Task<IEnumerable<CartItemsModel>> GetCart(Guid userId);
        Task UpdateCartItem(Guid userId, Guid productId, int quantity);
        Task RemoveCartItem(Guid userId, Guid productId);
        Task RemoveAllCartItems(Guid userId);
    }
}
