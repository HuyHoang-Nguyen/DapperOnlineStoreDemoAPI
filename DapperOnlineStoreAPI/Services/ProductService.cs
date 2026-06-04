using DapperOnlineStoreAPI.Entities;
using DapperOnlineStoreAPI.Enum.EnumError;
using DapperOnlineStoreAPI.GlobalExceptionHandler;
using DapperOnlineStoreAPI.IRepositories;
using DapperOnlineStoreAPI.Models;
using DapperOnlineStoreAPI.Services.Interfaces;
using DapperOnlineStoreAPI.Validators;

namespace DapperOnlineStoreAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<Guid> CreateAsync(ProductModel p)
        {
            var errors = new List<string>();
            var validateErrors = ProductValidator.ValidateCreate(p);
            errors.AddRange(validateErrors.Select(x => x.ToString()));

            if (!await _productRepository.CategoryCheckAsync(p.CategoryId))
            {
                errors.Add(EnumProductValidationError.CategoryInvalid.ToString());
            }
            if (errors.Any())
            {
                throw new ValidationException(errors);
            }
            return await _productRepository.CreateAsync(p);
        }
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _productRepository.GetAllAsync();
        }
        public async Task<Product?> GetByIdAsync(Guid id)
        {
            var p = await _productRepository.GetByIdAsync(id);
            if (p == null)
            {
                throw new ValidationException(new List<string>
                {
                    EnumProductValidationError.ProductNotFound.ToString(),
                });
            }
            return p;
        }
        public async Task<int> UpdateAsync(Guid id, UpdateProductModel p)
        {
            var errors = new List<string>();
            var validateErrors = ProductValidator.ValidateUpdate(p);
            errors.AddRange(validateErrors.Select(x => x.ToString()));

            var exists = await _productRepository.GetByIdAsync(id);
            if (exists == null)
            {
                throw new ValidationException(new List<string>
                {
                    EnumProductValidationError.ProductNotFound.ToString()
                });
            }
            if (p.CategoryId.HasValue && !await _productRepository.CategoryCheckAsync(p.CategoryId.Value))
            {
                errors.Add(EnumProductValidationError.CategoryInvalid.ToString());
            }
            if (errors.Any())
            {
                {
                    throw new ValidationException(errors);
                }
            }
            return await _productRepository.UpdateAsync(id, p);
        }
        public async Task<int> DeleteAsync(Guid id)
        {
            var exists = await _productRepository.GetByIdAsync(id);
            if (exists == null)
            {
                throw new ValidationException(new List<string>
                {
                    EnumProductValidationError.ProductNotFound.ToString()
                });
            }
            return await _productRepository.DeleteAsync(id);
        }
        public async Task<PagingResult<Product>> SearchAsync(string? keyword, Guid? categoryId, decimal? minPrice, decimal? maxPrice, int? minStock, int? maxStock, int page, int pageSize, string sortBy, string sortDir)
        {
            if (page < 1)
            {
                page = 1;
            }
            if (pageSize < 1)
            {
                pageSize = 10;
            }
            if (pageSize > 50)
            {
                pageSize = 50;
            }
            sortBy = sortBy?.ToLower() ?? "id";
            sortDir = sortDir?.ToLower() ?? "asc";
            return await _productRepository.SearchAsync(keyword, categoryId, minPrice, maxPrice, minStock, maxStock, page, pageSize, sortBy, sortDir);
        }

        public async Task<int> BulkUpdateEventAsync(BulkEventModel p)
        {
            var errors = new List<string>();
            if (p.ProductIds == null || !p.ProductIds.Any())
            {
                throw new ValidationException(new List<string>
                {
                    EnumProductValidationError.ProductIdsRequired.ToString()
                });
            }
            if (p.EventDiscount <= 0 || p.EventDiscount >= 100)
            {
                throw new ValidationException(new List<string>
                {
                    EnumProductValidationError.EventDiscountInvalid.ToString()
                });
            }
            if (p.EventStart.HasValue && p.EventEnd.HasValue && (p.EventStart.Value >= p.EventEnd.Value))
            {
                throw new ValidationException(new List<string>
                {
                    EnumProductValidationError.EventDateInvalid.ToString()
                });
            }
            if (errors.Any())
            {
                throw new ValidationException(errors);
            }
            return await _productRepository.BulkUpdateEventAsync(p.ProductIds, p.EventDiscount, p.EventStart, p.EventEnd);
        }
    }
}
