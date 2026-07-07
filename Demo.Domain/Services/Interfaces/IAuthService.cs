using Demo.Domain.Models;

namespace Demo.Domain.Services.Interfaces
{
    public interface IAuthService
    {
        Task LoginAsync(string email, string password);
        Task<UserLoginModel?> VerifyOTPAsync(string email, string code);
    }
}
