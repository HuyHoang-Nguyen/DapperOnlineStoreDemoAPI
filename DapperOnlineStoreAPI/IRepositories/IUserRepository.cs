using DapperOnlineStoreAPI.Entities;
using DapperOnlineStoreAPI.Models;

namespace DapperOnlineStoreAPI.IRepositories
{
    public interface IUserRepository
    {
        Task<Guid> CreateAsync(UserModel u);
        Task<bool> EmailExistedAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task<int> UpdateAsync(Guid id, UpdateUserModel u);
        Task<int> DeleteAsync(Guid id);
    }
}
