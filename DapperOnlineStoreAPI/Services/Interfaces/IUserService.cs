using DapperOnlineStoreAPI.Entities;
using DapperOnlineStoreAPI.Models;

namespace DapperOnlineStoreAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<Guid> CreateAsync(UserModel u);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task<int> UpdateAsync(Guid id, UpdateUserModel u);
        Task<int> DeleteAsync(Guid id);
    }
}
