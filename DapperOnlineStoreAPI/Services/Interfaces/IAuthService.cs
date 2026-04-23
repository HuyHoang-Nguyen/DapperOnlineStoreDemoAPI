using DapperOnlineStoreAPI.Models;

namespace DapperOnlineStoreAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserLoginModel?> LoginAsync(string email, string password);
    }
}
