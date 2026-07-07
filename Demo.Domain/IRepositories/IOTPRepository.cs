using Demo.Domain.Entities;
using Demo.Domain.Models;

namespace Demo.Domain.IRepositories
{
    public interface IOTPRepository
    {
        Task CreateAsync(string email, string code, DateTime expireAt);
        Task<OTPCode?> GetOTPAsync(string email, string code);
        Task UsedOTPAsync(Guid id);
    }
}
