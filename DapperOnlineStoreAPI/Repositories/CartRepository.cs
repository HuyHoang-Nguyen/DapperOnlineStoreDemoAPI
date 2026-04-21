using Dapper;
using DapperOnlineStoreAPI.IRepositories;
using DapperOnlineStoreAPI.Models;
using System.Data;

namespace DapperOnlineStoreAPI.Repositories
{
    public class CartRepository : BaseRepository, ICartRepository
    {
        private static readonly Guid TestUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        public CartRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task AddToCart(Guid productId, int quantity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("sp_AddToCart", new { UserId = TestUserId, ProductId = productId, Quantity = quantity }, commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CartItemsModel>> GetCart()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<CartItemsModel>("sp_GetCart", new { UserId = TestUserId }, commandType: CommandType.StoredProcedure);
        }
        public async Task UpdateCartItem(Guid productId, int quantityChange)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("sp_UpdateCartItem", new { UserId = TestUserId, productId = productId, quantity = quantityChange }, commandType: CommandType.StoredProcedure);
        }
        public async Task RemoveCartItem(Guid productId)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("sp_RemoveCartItem", new { UserId = TestUserId, productId = productId}, commandType: CommandType.StoredProcedure);
        }

    }
}

