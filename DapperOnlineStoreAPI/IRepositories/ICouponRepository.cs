using DapperOnlineStoreAPI.Entities;

namespace DapperOnlineStoreAPI.IRepositories
{
    public interface ICouponRepository
    {
        Task<Coupon?> GetByCodeAsync (string code);
        Task DescUsageLimitAsync(string couponId);
    }
}
