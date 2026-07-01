using Demo.Domain.Enum.EnumError;
using Demo.Domain.Models;

namespace Demo.Domain.Validators
{
    public class ProductValidator
    {
        public static List<EnumProductValidationError> ValidateCreate(ProductModel p)
        {
            var errors = new List<EnumProductValidationError>();
            if (p.CategoryId == Guid.Empty)
            {
                errors.Add(EnumProductValidationError.CategoryInvalid);
            }
            if (string.IsNullOrWhiteSpace(p.Name))
            {
                errors.Add(EnumProductValidationError.NameRequired);
            }
            if (p.Price == null)
            {
                errors.Add(EnumProductValidationError.PriceRequired);
            }
            else if (p.Price < 0)
            {
                errors.Add(EnumProductValidationError.PriceInvalid);
            }
            if (p.Stock == null)
            {
                errors.Add(EnumProductValidationError.StockREquired);
            }
            else if (p.Stock < 0)
            {
                errors.Add(EnumProductValidationError.StockInvalid);
            }
            if (p.Discount != null && (p.Discount < 0 || p.Discount > 100))
            {
                errors.Add(EnumProductValidationError.DiscountInvalid);
            }
            return errors;
        }
        public static List<EnumProductValidationError> ValidateUpdate(UpdateProductModel p)
        {
            var errors = new List<EnumProductValidationError>();
            if (p.CategoryId.HasValue && p.CategoryId == Guid.Empty)
            {
                errors.Add(EnumProductValidationError.CategoryInvalid);
            }
            if (p.Name != null && string.IsNullOrWhiteSpace(p.Name))
            {
                errors.Add(EnumProductValidationError.NameRequired);
            }
            if (p.Price.HasValue && p.Price.Value < 0)
            {
                errors.Add(EnumProductValidationError.PriceInvalid);
            }
            if (p.Stock.HasValue && p.Stock.Value < 0)
            {
                errors.Add(EnumProductValidationError.StockInvalid);
            }
            if (p.Discount.HasValue && (p.Discount.Value < 0 || p.Discount.Value > 100))
            {
                errors.Add (EnumProductValidationError.DiscountInvalid);
            }
            return errors;
        }

    }
}
