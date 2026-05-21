using Dapper;
using DapperOnlineStoreAPI.Entities;
using DapperOnlineStoreAPI.Enum;
using DapperOnlineStoreAPI.IRepositories;
using DapperOnlineStoreAPI.Models;
using System.Data;

namespace DapperOnlineStoreAPI.Repositories
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        public OrderRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task<Guid> CreateOrderAsync (Guid userId, IEnumerable<CartItemsModel> cartItems, string? couponCode , decimal discountAmount)
        {
            using var connection = CreateConnection();
            const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            string code;
            bool codeExists;
            do
            {
                code = new string(Enumerable
                    .Range(0, 10)
                    .Select(_ => Chars[random.Next(Chars.Length)])
                    .ToArray());
                codeExists = await CodeExistsAsync(code);
            }
            while (codeExists);

            var items = cartItems.ToList();
            var order = new Order
            {
                Id = Guid.NewGuid(),
                Code = code,
                UserId = userId,
                TotalAmount = items.Sum(x => (x.DiscountPrice ?? x.Price) * x.Quantity),
                CouponCode = couponCode,
                DiscountAmount = discountAmount,
                Status = EnumOrderStatus.Created,
            };
            var createOrderSql = "insert into Orders(Id, Code, UserId, TotalAmount, DiscountAmount, CouponCode, Status) " +
                                 "values (@Id, @Code, @UserId, @TotalAmount, @DiscountAmount, @CouponCode, @Status) ";

            await connection.ExecuteAsync(createOrderSql, order);

            var orderItems = items.Select(i => new OrderItem
            {
                OrderId = order.Id,
                ProductId = i.ProductId,
                ProductName = i.Name,
                Price = i.DiscountPrice ?? i.Price,
                Quantity = i.Quantity,
                ImageUrl = i.ImageUrl,
            }).ToList();
            var insertOrderItemSql = "insert into OrderItems (OrderId, ProductId, ProductName, Price, Quantity) " +
                                     "values (@OrderId, @ProductId, @ProductName, @Price, @Quantity) ";
            await connection.ExecuteAsync(insertOrderItemSql, orderItems);
            var updateStockSql = "update Products " +
                                 "set Stock = Stock - @Quantity " +
                                 "where Id = @ProductId and IsDeleted = 0";
            await connection.ExecuteAsync(updateStockSql, orderItems);
            return order.Id;
        }
        public async Task<IEnumerable<Order>> GetOrdersAsync(Guid userId)
        {
            using var connection = CreateConnection();
            var sql = "select Id, Code, UserId, TotalAmount, Status, CreatedDate " +
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
            var items = await connection.QueryAsync<OrderItem>(sql, new { OrderId = orderId });
            return items.Select(i =>
            {
                i.ImageUrls = (i.ImageUrl ?? "").Split('|', StringSplitOptions.RemoveEmptyEntries).ToList();
                return i;
            });
        }
        public async Task DeleteOrderAsync(Guid orderId, Guid userId)
        {
            using var connection = CreateConnection();
            await connection.ExecuteAsync("sp_DeleteOrder", new { OrderId = orderId, UserId = userId }, commandType: CommandType.StoredProcedure);
        }
        public async Task<bool> CodeExistsAsync(string code)
        {
            using var connection = CreateConnection();
            var sql = "select top 1 1 from Orders where Code = @Code ";
            var result = await connection.QueryFirstOrDefaultAsync<int?>(sql, new { Code = code });
            return result.HasValue;
        }
    }
}
