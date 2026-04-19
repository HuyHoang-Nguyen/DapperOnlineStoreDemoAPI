using DapperOnlineStoreAPI.Enum.EnumError;
using DapperOnlineStoreAPI.Models;

namespace DapperOnlineStoreAPI.Validators
{
    public class ProductValidator
    {
        public static List<EnumProductValidationError> ValidateCommon(Guid? categoryId, string? name, decimal? price, int? stock)
        {
            var errors = new List<EnumProductValidationError>();
            if (categoryId == null)
            {
                errors.Add(EnumProductValidationError.CategoryInvalid);
            }
            if (string.IsNullOrWhiteSpace(name))
            {
                errors.Add(EnumProductValidationError.NameRequired);
            }
            if (price == null)
            {
                errors.Add(EnumProductValidationError.PriceRequired);
            }
            else if (price < 0)
            {
                errors.Add(EnumProductValidationError.PriceInvalid);
            }
            if (stock == null)
            {
                errors.Add(EnumProductValidationError.StockREquired);
            }
            else if (stock < 0)
            {
                errors.Add(EnumProductValidationError.StockInvalid);
            }
            return errors;
        }
        public static List<EnumProductValidationError> ValidateCreate(ProductModel p)
        {
            return ValidateCommon(p.CategoryId, p.Name, p.Price, p.Stock);
        }
        public static List<EnumProductValidationError> ValidateUpdate(UpdateProductModel p)
        {
            return ValidateCommon(p.CategoryId, p.Name, p.Price, p.Stock);
        }
    }
}
