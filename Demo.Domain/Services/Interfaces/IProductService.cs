using Demo.Domain.Entities;
using Demo.Domain.Models;

namespace Demo.Domain.Services.Interfaces
{
    public interface IProductService
    {
        Task<Guid> CreateAsync(ProductModel p);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(Guid id);
        Task<int> UpdateAsync(Guid id, UpdateProductModel p);
        Task<int> DeleteAsync(Guid id);
        Task<PagingResult<Product>> SearchAsync(string? keyword, Guid? categoryId, decimal? minPrice, decimal? maxPrice, int? minStock, int? maxStock, int page, int pageSize, string sortBy, string sortDir);
        Task<int> BulkUpdateEventAsync(BulkEventModel model);

    }
}
