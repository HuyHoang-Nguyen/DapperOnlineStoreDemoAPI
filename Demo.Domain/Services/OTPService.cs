using Demo.Domain.IRepositories;
using Demo.Domain.Services.Interfaces;

namespace Demo.Domain.Services
{
    public class OTPService : IOTPService
    {
        private readonly IOTPRepository _otpRepository;

        public OTPService(IOTPRepository otpRepository)
        {
            _otpRepository = otpRepository;
        }
        public async Task<string> GenerateOTPAsync(string email)
        {
            var code = Random.Shared.Next(100000, 999999).ToString();
            var expireAt = DateTime.Now.AddMinutes(10);

            await _otpRepository.CreateAsync(email, code, expireAt);
            return code;
        }
        public async Task<bool> ValidateAsync(string email, string code)
        {
            var otp = await _otpRepository.GetOTPAsync(email, code);
            if (otp == null)
            {
                return false;
            }
            await _otpRepository.UsedOTPAsync(otp.Id);
            return true;
        }
    }
}
