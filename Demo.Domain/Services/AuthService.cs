using Demo.Domain.Enum.EnumError;
using Demo.Domain.GlobalExceptionHandler;
using Demo.Domain.IRepositories;
using Demo.Domain.Models;
using Demo.Domain.Services.Interfaces;

namespace Demo.Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IOTPService _otpService;
        public AuthService(IUserRepository userRepository, IJwtService jwtService, IOTPService otpService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _otpService = otpService;
        }
        public async Task<string> LoginAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new ValidationException(new List<string>
                {
                    EnumLoggingValidationError.LoginFailed.ToString()
                });
            }
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || user.Password != password)
            {
                throw new ValidationException(new List<string>
                {
                    EnumLoggingValidationError.LoginFailed.ToString()
                });
            }
            await _otpService.GenerateOTPAsync(email);

            var code = await _otpService.GenerateOTPAsync(email);
            return code;
        }
        public async Task<UserLoginModel?> VerifyOTPAsync(string email, string code)
        {
            var isValid = await _otpService.ValidateAsync(email, code);
            if (!isValid)
            {
                throw new ValidationException(new List<string>
                {
                    EnumLoggingValidationError.LoginFailed.ToString()
                });
            }

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                throw new ValidationException(new List<string>
                {
                    EnumLoggingValidationError.LoginFailed.ToString()
                });
            }

            IList<string>? roles = user.Role != null ? new List<string> { user.Role } : null;
            var token = _jwtService.GenerateToken(user.Id, user.Email, roles);

            return new UserLoginModel
            {
                Id = user.Id,
                Email = email,
                Token = token
            };
        }
    }
}
