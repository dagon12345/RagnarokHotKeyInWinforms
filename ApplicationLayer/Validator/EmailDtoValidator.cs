using ApplicationLayer.Dto;
using FluentValidation;

namespace ApplicationLayer.Validator
{
    public class EmailDtoValidator : AbstractValidator<EmailDto>
    {
        public EmailDtoValidator()
        {
            RuleFor(x => x.Email)
               .NotEmpty()
               .NotNull()
               .EmailAddress().WithMessage("Invalid email address format.");
        }
    }
}
