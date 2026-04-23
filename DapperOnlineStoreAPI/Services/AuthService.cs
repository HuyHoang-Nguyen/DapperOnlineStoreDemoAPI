using DapperOnlineStoreAPI.Enum.EnumError;
using DapperOnlineStoreAPI.GlobalExceptionHandler;
using DapperOnlineStoreAPI.IRepositories;
using DapperOnlineStoreAPI.Models;
using DapperOnlineStoreAPI.Services.Interfaces;

namespace DapperOnlineStoreAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserLoginModel?> LoginAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
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
                    EnumUserValidationError.UserNotFound.ToString()
                });
            }
            if (user.Password != password)
            {
                throw new ValidationException(new List<string>
                {
                    EnumLoggingValidationError.LoginFailed.ToString()
                });
            }
            return new UserLoginModel
            {
                Id = user.Id,
                Email = email,
            };
        }
    }
}
