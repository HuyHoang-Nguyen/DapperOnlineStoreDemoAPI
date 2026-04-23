using DapperOnlineStoreAPI.Entities;
using DapperOnlineStoreAPI.Enum.EnumError;
using DapperOnlineStoreAPI.GlobalExceptionHandler;
using DapperOnlineStoreAPI.IRepositories;
using DapperOnlineStoreAPI.Models;
using DapperOnlineStoreAPI.Services.Interfaces;
using DapperOnlineStoreAPI.Validators;

namespace DapperOnlineStoreAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Guid> CreateAsync(UserModel u)
        {
            var errors = new List<string>();

            var validateErrors = UserValidator.ValidateCreate(u);
            errors.AddRange(validateErrors.Select(x => x.ToString()));

            if (!string.IsNullOrEmpty(u.Email) && await _userRepository.EmailExistedAsync(u.Email))
            {
                errors.Add(EnumUserValidationError.EmailExisted.ToString());
            }
            if (errors.Any())
            {
                throw new ValidationException(errors);
            }
            return await _userRepository.CreateAsync(u);
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _userRepository.GetAllAsync();
        }
        public async Task<User?> GetByIdAsync(Guid id)
        {
            var u = await _userRepository.GetByIdAsync(id);
            if (u == null)
            {
                throw new ValidationException(new List<string>
                {
                    EnumUserValidationError.UserNotFound.ToString()
                });
            }
            return u;
        }
        public async Task<int> UpdateAsync(Guid id, UpdateUserModel u)
        {
            var errors = new List<string>();
            var validateErrors = UserValidator.ValidateUpdate(u);
            errors.AddRange(validateErrors.Select(x => x.ToString()));

            var exists = await _userRepository.GetByIdAsync(id);
            if (exists == null)
            {
                errors.Add(EnumUserValidationError.UserNotFound.ToString());
            }
            if (errors.Any())
            {
                throw new ValidationException(errors);
            }
            return await _userRepository.UpdateAsync(id, u);
        }
        public async Task<int> DeleteAsync(Guid id)
        {
            var exists = _userRepository.GetByIdAsync(id);
            if (exists == null)
            {
                throw new ValidationException(new List<string>
                {
                    EnumUserValidationError.UserNotFound.ToString()
                });
            }
            return await _userRepository.DeleteAsync(id);
        }
        public async Task<User?> GetByEmailAsync (string email)
        {
            var u = await _userRepository.GetByEmailAsync(email);
            if (u == null)
            {
                throw new ValidationException(new List<string>
                {
                    EnumUserValidationError.UserNotFound.ToString()
                });
            }
            return u;
        }
    }
}
