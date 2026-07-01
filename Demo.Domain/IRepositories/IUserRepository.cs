using Demo.Domain.Entities;
using Demo.Domain.Models;

namespace Demo.Domain.IRepositories
{
    public interface IUserRepository
    {
        Task<Guid> CreateAsync(UserModel u);
        Task<bool> EmailExistedAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task<int> UpdateAsync(Guid id, UpdateUserModel u);
        Task<int> DeleteAsync(Guid id);
        Task<User?> GetByEmailAsync (string email);
    }
}
