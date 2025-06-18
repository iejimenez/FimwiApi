using FluentValidation;
using FimwiApi.Core.Entities;

namespace FimwiApi.Core.Validators
{
    public class InvoiceItemValidator : AbstractValidator<InvoiceItem>
    {
        public InvoiceItemValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0);

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0)
                .PrecisionScale(18, 2, false);

            RuleFor(x => x.Subtotal)
                .GreaterThan(0)
                .PrecisionScale(18, 2, false)
                .Equal(x => x.Quantity * x.UnitPrice)
                .WithMessage("Subtotal must be equal to Quantity * UnitPrice");
        }
    }
} 