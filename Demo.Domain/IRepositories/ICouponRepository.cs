using Demo.Domain.Entities;

namespace Demo.Domain.IRepositories
{
    public interface ICouponRepository
    {
        Task<Coupon?> GetByCodeAsync (string code);
        Task DescUsageLimitAsync(Guid couponId);
    }
}
