namespace Demo.Domain.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendOTPAsync(string toEmail, string otpCode);
    }
}
