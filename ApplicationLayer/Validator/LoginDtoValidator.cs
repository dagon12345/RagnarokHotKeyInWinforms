using ApplicationLayer.Dto;
using FluentValidation;
using System.Text.RegularExpressions;

namespace ApplicationLayer.Validator
{
    public class LoginDtoValidator: AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress().WithMessage("Invalid email address format."); ;
            RuleFor(x => x.Password)
                .NotEmpty()
                .NotNull();  
        }

    }
}
