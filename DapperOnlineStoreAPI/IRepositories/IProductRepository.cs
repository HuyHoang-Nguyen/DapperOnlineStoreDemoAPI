using DapperOnlineStoreAPI.Entities;
using DapperOnlineStoreAPI.Models;

namespace DapperOnlineStoreAPI.IRepositories
{
    public interface IProductRepository
    {
        Task<Guid> CreateAsync(ProductModel p);
        Task<bool> CategoryCheckAsync(Guid categoryId);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(Guid id);
        Task<int> UpdateAsync(Guid id, UpdateProductModel p);
        Task<int> DeleteAsync(Guid id);
        Task<PagingResult<Product>> SearchAsync(string? keyword, Guid? categoryId, decimal? minPrice, decimal? maxPrice, int? minStock, int? maxStock, int page, int pageSize, string sortBy, string sortDir);

    }
}
