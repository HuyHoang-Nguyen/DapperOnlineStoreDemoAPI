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
        //public async Task AddToCart(Guid productId, int quantity)
        //{
        //    using var connection = CreateConnection();
        //    await connection.QueryAsync("sp_AddToCart", new { UserId = TestUserId, ProductId = productId, Quantity = quantity }, commandType : CommandType.StoredProcedure);
        //} 

        public async Task AddToCart(Guid productId, int quantity)
        {
            using var connection = CreateConnection();

            using var multi = await connection.QueryMultipleAsync(
                "sp_AddToCart",
                new { UserId = TestUserId, ProductId = productId, Quantity = quantity },
                commandType: CommandType.StoredProcedure
            );

            var db = await multi.ReadAsync();
            var productIdDebug = await multi.ReadAsync();
            var product = await multi.ReadAsync();

            Console.WriteLine("DB:");
            foreach (var d in db) Console.WriteLine(d);

            Console.WriteLine("ProductId:");
            foreach (var d in productIdDebug) Console.WriteLine(d);

            Console.WriteLine("Product:");
            foreach (var d in product) Console.WriteLine(d);
        }
        public async Task<IEnumerable<CartItemsModel>> GetCart()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<CartItemsModel>("sp_GetCart", new { UserId = TestUserId }, commandType: CommandType.StoredProcedure);
        }
    }
}

