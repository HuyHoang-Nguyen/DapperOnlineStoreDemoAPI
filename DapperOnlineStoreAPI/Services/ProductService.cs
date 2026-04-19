using DapperOnlineStoreAPI.Entities;
using DapperOnlineStoreAPI.Enum.EnumError;
using DapperOnlineStoreAPI.GlobalExceptionHandler;
using DapperOnlineStoreAPI.IRepositories;
using DapperOnlineStoreAPI.Models;
using DapperOnlineStoreAPI.Repositories;
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

            var exists = _productRepository.GetByIdAsync(id);
            if (exists == null)
            {
                throw new ValidationException(new List<string>
                {
                    EnumProductValidationError.ProductNotFound.ToString()
                });
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
            var exists = _productRepository.GetByIdAsync(id);
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
            return await _productRepository.SearchAsync(keyword, categoryId, minPrice, maxPrice, minStock, maxStock, page, pageSize, sortBy, sortDir);
        }
    }
}
