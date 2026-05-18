using DapperOnlineStoreAPI.Entities;

namespace DapperOnlineStoreAPI.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(Guid id);
    }
}
