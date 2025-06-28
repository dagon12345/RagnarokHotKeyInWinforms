using ApplicationLayer.Dto;
using FluentValidation;
using System.Linq;
using System.Text.RegularExpressions;

namespace ApplicationLayer.Validator
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator()
        {
            RuleFor(x => x.Email)
           .NotEmpty().WithMessage("Email is required.")
           .MaximumLength(254).WithMessage("Email must not exceed 254 characters.")
           .EmailAddress().WithMessage("Invalid email address format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .MaximumLength(100).WithMessage("Password must not exceed 100 characters.")
                .Must(ContainUppercase).WithMessage("Password must contain at least one uppercase letter.")
                .Must(ContainLowercase).WithMessage("Password must contain at least one lowercase letter.")
                .Must(ContainDigit).WithMessage("Password must contain at least one number.")
                .Must(ContainSpecial).WithMessage("Password must contain at least one special character (e.g. !@#$%^&*).");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Passwords do not match.");
        }
        private bool ContainUppercase(string password) =>
      !string.IsNullOrEmpty(password) && password.Any(char.IsUpper);

        private bool ContainLowercase(string password) =>
            !string.IsNullOrEmpty(password) && password.Any(char.IsLower);

        private bool ContainDigit(string password) =>
            !string.IsNullOrEmpty(password) && password.Any(char.IsDigit);

        private bool ContainSpecial(string password) =>
            !string.IsNullOrEmpty(password) && Regex.IsMatch(password, @"[!@#\$%\^&\*\(\)_\+\-=

\[\]

{};':"".,<>\/?\\|`~]");
    }
}
