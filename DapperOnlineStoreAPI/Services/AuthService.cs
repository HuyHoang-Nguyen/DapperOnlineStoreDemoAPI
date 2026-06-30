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
        private readonly IJwtService _jwtService;
        public AuthService(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
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
            if (user == null || user.Password != password)
            {
                throw new ValidationException(new List<string>
                {
                    EnumLoggingValidationError.LoginFailed.ToString()
                });
            }
            var token = _jwtService.GenerateToken(user.Id, user.Email);
            return new UserLoginModel
            {
                Id = user.Id,
                Email = email,
                Token = token
            };           
        }
    }
}
