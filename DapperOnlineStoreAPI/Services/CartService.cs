using DapperOnlineStoreAPI.Enum.EnumError;
using DapperOnlineStoreAPI.GlobalExceptionHandler;
using DapperOnlineStoreAPI.IRepositories;
using DapperOnlineStoreAPI.Models;
using DapperOnlineStoreAPI.Services.Interfaces;

namespace DapperOnlineStoreAPI.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }
        public async Task<IEnumerable<CartItemsModel>> GetCart()
        {
            return await _cartRepository.GetCart();
        }
        public async Task AddToCart(Guid productId, int quantity)
        {
            var stock = await _productRepository.GetStockAsync(productId);
            if (productId == Guid.Empty)
            {
                throw new ValidationException(new List<string>
                {
                    EnumCartValidationError.ProductIdInvalid.ToString()
                });
            }
            if (quantity <= 0)
            {
                throw new ValidationException(new List<string>
                {
                    EnumCartValidationError.QuantityInvalid.ToString()
                });
            }
            if (quantity > stock)
            {
                throw new ValidationException(new List<string>
                {
                    EnumCartValidationError.OutOfStock.ToString()
                });
            }
            await _cartRepository.AddToCart(productId, quantity);
        }
        public async Task UpdateCartItem(Guid productId, int quantityChange)
        {
            if (productId == Guid.Empty)
            {
                throw new ValidationException(new List<string>
                {
                    EnumCartValidationError.ProductIdInvalid.ToString()
                });
            }
            await _cartRepository.UpdateCartItem(productId, quantityChange);
        }
        public async Task RemoveCartItem(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                throw new ValidationException(new List<string>
                {
                    EnumCartValidationError.ProductIdInvalid.ToString()
                });
            }
            await _cartRepository.RemoveCartItem(productId);
        }
    }
}
