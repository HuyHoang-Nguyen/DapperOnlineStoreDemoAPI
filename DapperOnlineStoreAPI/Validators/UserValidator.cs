using DapperOnlineStoreAPI.Enum.EnumError;
using DapperOnlineStoreAPI.Models;
using System.Text.RegularExpressions;

namespace DapperOnlineStoreAPI.Validators
{
    public static class UserValidator
    {
        public static List<EnumUserValidationError> ValidateCommon(string? email, string? password)
        {
            var errors = new List<EnumUserValidationError>();

            if (string.IsNullOrWhiteSpace(email))
            {
                errors.Add(EnumUserValidationError.EmailRequired);
            }
            else if (!email.Contains("@") || !email.Contains("."))
            {
                errors.Add(EnumUserValidationError.EmailInvalid);
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add(EnumUserValidationError.PasswordRequired);
            }
            else
            {
                if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
                {
                    errors.Add(EnumUserValidationError.PasswordInvalid);
                }
            }
            return errors;
        }
        public static List<EnumUserValidationError> ValidateCreate(UserModel u)
        {
            return ValidateCommon(u.Email, u.Password);
        }
        public static List<EnumUserValidationError> ValidateUpdate(UpdateUserModel u)
        {
            var errors = new List<EnumUserValidationError>();

            if (string.IsNullOrEmpty(u.Password))
            {
                errors.Add(EnumUserValidationError.PasswordRequired);
            }
            else if (!Regex.IsMatch(u.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
            {
                errors.Add(EnumUserValidationError.PasswordInvalid);
            }
            return errors;
        }
    }
}
