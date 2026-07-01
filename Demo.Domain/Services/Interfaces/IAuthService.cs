using Demo.Domain.Models;

namespace Demo.Domain.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserLoginModel?> LoginAsync(string email, string password);
    }
}
