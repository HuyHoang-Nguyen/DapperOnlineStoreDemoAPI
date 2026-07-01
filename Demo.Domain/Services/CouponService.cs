using Demo.Domain.Enum.EnumError;
using Demo.Domain.GlobalExceptionHandler;
using Demo.Domain.IRepositories;
using Demo.Domain.Models;
using Demo.Domain.Services.Interfaces;

namespace Demo.Domain.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;
        private readonly ICartRepository _cartRepository;
        public CouponService(ICouponRepository couponRepository, ICartRepository cartRepository)
        {
            _couponRepository = couponRepository;
            _cartRepository = cartRepository;
        }
        public async Task<CouponModel> ValidateAsync(string code, decimal cartTotal, Guid userId)
        {
            var coupon = await _couponRepository.GetByCodeAsync(code);
            var cartItems = await _cartRepository.GetCart(userId);
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
            if (coupon.ExpireDate.HasValue && coupon.ExpireDate < DateTime.Now)
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
            if (coupon.MinOrderAmount.HasValue && cartItems.Sum(i => i.Quantity) < coupon.MinOrderAmount.Value)
            {
                throw new ValidationException(new List<string>
                {
                    EnumCouponValidationError.CouponConditionNotMet.ToString()
                });
            }
            if (coupon.CategoryId.HasValue)
            {
                var categoryCheck = cartItems.Any(i => i.CategoryId == coupon.CategoryId.Value);
                if (!categoryCheck)
                {
                    throw new ValidationException(new List<string>
                    { 
                        EnumCouponValidationError.CouponConditionNotMet.ToString() 
                    });
                }
            }
            if (coupon.MinTotalAmount.HasValue && cartTotal < coupon.MinTotalAmount)
            {
                throw new ValidationException(new List<string>
                {
                    EnumCouponValidationError.CouponConditionNotMet.ToString()
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
