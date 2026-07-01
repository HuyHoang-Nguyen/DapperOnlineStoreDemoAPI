using Demo.Domain.Models;

namespace Demo.Domain.Services.Interfaces
{
    public interface ICouponService
    {
        Task<CouponModel> ValidateAsync(string code, decimal cartTotal, Guid userId);
    }
}
