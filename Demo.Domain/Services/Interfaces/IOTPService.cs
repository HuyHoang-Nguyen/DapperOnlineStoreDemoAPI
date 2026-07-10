namespace Demo.Domain.Services.Interfaces
{
    public interface IOTPService
    {
        Task<string> GenerateOTPAsync(string email);
        Task<bool> ValidateAsync(string email, string code);
    }
}
