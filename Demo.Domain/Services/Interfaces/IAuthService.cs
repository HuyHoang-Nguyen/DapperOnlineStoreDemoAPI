using Demo.Domain.Models;

namespace Demo.Domain.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(string email, string password);
        Task<UserLoginModel?> VerifyOTPAsync(string email, string code);
    }
}
