using DapperOnlineStoreAPI.Models;

namespace DapperOnlineStoreAPI.Services.Interfaces
{
    public interface ICouponService
    {
        Task<CouponModel> ValidateAsync(string code, decimal cartTotal);
    }
}
