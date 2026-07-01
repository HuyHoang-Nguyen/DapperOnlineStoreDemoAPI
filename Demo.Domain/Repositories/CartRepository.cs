using Dapper;
using Demo.Domain.IRepositories;
using Demo.Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Demo.Domain.Repositories
{
    public class CartRepository : BaseRepository, ICartRepository
    {
        public CartRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task AddToCart(Guid userId, Guid productId, int quantity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("sp_AddToCart", new { UserId = userId, ProductId = productId, Quantity = quantity }, commandType: CommandType.StoredProcedure);
        }
        public async Task<IEnumerable<CartItemsModel>> GetCart(Guid userId)
        {
            using var connection = CreateConnection();
            var items = await connection.QueryAsync<CartItemsModel>("sp_GetCart", new { UserId = userId }, commandType: CommandType.StoredProcedure);
            return items.Select(i =>
            {
                i.ImageUrls = (i.ImageUrl ?? "").Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();
                return i;
            });
        }
        public async Task UpdateCartItem(Guid userId, Guid productId, int quantity)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("sp_UpdateCartItem", new { UserId = userId, productId, quantity }, commandType: CommandType.StoredProcedure);
        }
        public async Task RemoveCartItem(Guid userId, Guid productId)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("sp_RemoveCartItem", new { UserId = userId, productId}, commandType: CommandType.StoredProcedure);
        }
        public async Task RemoveAllCartItems(Guid userId)
        {
            using var connection = CreateConnection();
            var sql = "delete ci " +
                      "from CartItems ci " +
                      "join Carts c on c.Id = ci.CartId " +
                      "where c.UserId = @UserId";
            await connection.ExecuteAsync(sql, new { UserId = userId });
        }
    }
}

