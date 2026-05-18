using DapperOnlineStoreAPI.Entities;

namespace DapperOnlineStoreAPI.IRepositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(Guid id);
    }
}
