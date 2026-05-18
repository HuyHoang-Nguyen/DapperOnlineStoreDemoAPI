using Dapper;
using DapperOnlineStoreAPI.Entities;
using DapperOnlineStoreAPI.IRepositories;
using DapperOnlineStoreAPI.Models;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Data;

namespace DapperOnlineStoreAPI.Repositories
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        public OrderRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task<Guid> CreateOrderAsync (Guid userId, IEnumerable<CartItemsModel> cartItems)
        {
            using var connection = CreateConnection();
            var items = cartItems.ToList();
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TotalAmount = items.Sum(x => x.GrandTotal),
                Status = 1,
            };
            var createOrderSql = "insert into Orders(Id, UserId, TotalAmount, Status) " +
                                 "values (@Id, @UserId, @TotalAmount, @Status) ";

            await connection.ExecuteAsync(createOrderSql, order);

            var orderItems = items.Select(i => new OrderItem
            {
                OrderId = order.Id,
                ProductId = i.ProductId,
                ProductName = i.Name,
                Price = i.Price,
                Quantity = i.Quantity,
            }).ToList();
            var insertOrderItemSql = "insert into OrderItems (OrderId, ProductId, ProductName, Price, Quantity) " +
                                     "values (@OrderId, @ProductId, @ProductName, @Price, @Quantity) ";
            await connection.ExecuteAsync(insertOrderItemSql, orderItems);
            return order.Id;
        }
        public async Task<IEnumerable<Order>> GetOrdersAsync(Guid userId)
        {
            using var connection = CreateConnection();
            var sql = "select Id, UserId, TotalAmount, Status, CreatedDate " +
                      "from Orders " +
                      "where UserId = @UserId and IsDeleted = 0 " +
                      "order by CreatedDate desc ";
            return await connection.QueryAsync<Order>(sql, new{ UserId = userId });
        }
        public async Task<Order?> GetByIdAsync(Guid orderId, Guid userId)
        {
            using var connection = CreateConnection();
            var sql = "select Id, TotalAmount, Status, CreatedDate " +
                      "from Orders " +
                      "where Id = @OrderId and UserId = @UserId and IsDeleted = 0 ";
            return await connection.QueryFirstOrDefaultAsync<Order>(sql, new { OrderId = orderId, UserId = userId });
        }
        public async Task<IEnumerable<OrderItem>> GetOrderItemsAsync(Guid orderId)
        {
            using var connection = CreateConnection();
            var sql = "select oi.OrderId, oi.ProductId, oi.ProductName, oi.Price, oi.Quantity, p.ImageUrl " +
                      "from OrderItems oi " +
                      "join Products p on p.Id = oi.ProductId " +
                      "where oi.OrderId = @OrderId and oi.IsDeleted = 0 ";
            return await connection.QueryAsync<OrderItem>(sql, new { OrderId = orderId });
        }
        public async Task DeleteOrderAsync(Guid orderId, Guid userId)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("sp_DeleteOrder", new { OrderId = orderId, UserId = userId }, commandType: CommandType.StoredProcedure);
        }
    }
}
