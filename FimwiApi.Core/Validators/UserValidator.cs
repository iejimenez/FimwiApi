using FluentValidation;
using FimwiApi.Core.Entities;

namespace FimwiApi.Core.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
           
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100);

            RuleFor(x => x.PasswordHash)
                .NotEmpty();

            RuleFor(x => x.RoleId)
                .NotEmpty()
                .WithMessage("Role is required");
        }
    }
} 