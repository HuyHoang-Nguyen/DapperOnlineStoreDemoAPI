using DapperOnlineStoreAPI.Enum.EnumError;
using DapperOnlineStoreAPI.GlobalExceptionHandler;
using DapperOnlineStoreAPI.IRepositories;
using DapperOnlineStoreAPI.Models;
using DapperOnlineStoreAPI.Services.Interfaces;

namespace DapperOnlineStoreAPI.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;
        public CouponService(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }
        public async Task<CouponModel> ValidateAsync(string code, decimal cartTotal)
        {
            var coupon = await _couponRepository.GetByCodeAsync(code);
            if (coupon == null)
            {
                throw new ValidationException(new List<string>
                {
                    EnumCouponValidationError.CouponNotFound.ToString()
                });
            }
            if (!coupon.IsActive)
            {
                throw new ValidationException(new List<string>
                {
                    EnumCouponValidationError.CouponInactive.ToString()
                });
            }
            if (coupon.ExpireDate.HasValue && coupon.ExpireDate < DateTime.UtcNow)
            {
                throw new ValidationException(new List<string>
                {
                    EnumCouponValidationError.CouponExpired.ToString()
                });
            }
            if (coupon.UsageLimit.HasValue && coupon.UsageLimit <= 0)
            {
                throw new ValidationException(new List<string>
                {
                    EnumCouponValidationError.CouponLimitReached.ToString()
                });
            }
            var discountAmount = coupon.DiscountType == "Percentage" ? cartTotal * (coupon.DiscountValue / 100) : coupon.DiscountValue;
            discountAmount = Math.Min(discountAmount, cartTotal);
            return new CouponModel
            {
                Code = coupon.Code,
                DiscountType = coupon.DiscountType,
                DiscountValue = coupon.DiscountValue,
                DiscountAmount = discountAmount,
                FinalAmount = cartTotal - discountAmount
            };
        }
    }
}
