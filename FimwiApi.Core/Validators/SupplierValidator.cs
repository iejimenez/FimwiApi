using FluentValidation;
using FimwiApi.Core.Entities;

namespace FimwiApi.Core.Validators
{
    public class SupplierValidator : AbstractValidator<Supplier>
    {
        public SupplierValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.ContactName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(100);

            RuleFor(x => x.Phone)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.Address)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.TaxId)
                .NotEmpty()
                .MaximumLength(20);
        }
    }
} 