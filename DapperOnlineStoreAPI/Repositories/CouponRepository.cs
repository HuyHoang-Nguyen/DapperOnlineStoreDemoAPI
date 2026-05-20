using Dapper;
using DapperOnlineStoreAPI.Entities;
using DapperOnlineStoreAPI.IRepositories;

namespace DapperOnlineStoreAPI.Repositories
{
    public class CouponRepository : BaseRepository, ICouponRepository
    {
        public CouponRepository(IConfiguration configuration) : base(configuration)
        {
        }
        public async Task<Coupon?> GetByCodeAsync(string code)
        {
            using var connection = CreateConnection();
            var sql = "select Id, Code, DiscountType, DiscountValue, ExpireDate, UsageLimit, IsActive " +
                      "from Coupon where Code = @Code and IsDeleted = 0 ";
            return await connection.QueryFirstOrDefaultAsync(sql, new { Code = code });
        }
        public async Task DescUsageLimitAsync(string couponId)
        {
            using var connection = CreateConnection();
            var sql = "update Coupon set UsageLimit = UsageLimit - 1 where Id = @Id ";
            await connection.ExecuteAsync(sql, new { Id = couponId });
        }
    }
}
