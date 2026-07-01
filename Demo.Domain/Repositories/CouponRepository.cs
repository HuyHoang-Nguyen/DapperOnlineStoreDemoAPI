using Dapper;
using Demo.Domain.Entities;
using Demo.Domain.IRepositories;
using Microsoft.Extensions.Configuration;

namespace Demo.Domain.Repositories
{
    public class CouponRepository : BaseRepository, ICouponRepository
    {
        public CouponRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task<Coupon?> GetByCodeAsync(string code)
        {
            using var connection = CreateConnection();
            var sql = "select Id, Code, DiscountType, DiscountValue, ExpireDate, UsageLimit, IsActive, MinOrderAmount, CategoryId, MinTotalAmount " +
                      "from Coupon where Code = @Code and IsDeleted = 0 ";
            return await connection.QueryFirstOrDefaultAsync<Coupon>(sql, new { Code = code });
        }
        public async Task DescUsageLimitAsync(Guid couponId)
        {
            using var connection = CreateConnection();
            var sql = "update Coupon set UsageLimit = UsageLimit - 1 where Id = @Id ";
            await connection.ExecuteAsync(sql, new { Id = couponId });
        }
    }
}
