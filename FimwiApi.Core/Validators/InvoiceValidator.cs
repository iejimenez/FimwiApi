using FluentValidation;
using FimwiApi.Core.Entities;

namespace FimwiApi.Core.Validators
{
    public class InvoiceValidator : AbstractValidator<Invoice>
    {
        public InvoiceValidator()
        {
            RuleFor(x => x.InvoiceNumber)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.Date)
                .NotEmpty();

            RuleFor(x => x.Status)
                .NotEmpty()
                .Must(status => status == "draft" || status == "sent" || status == "paid" || status == "cancelled")
                .WithMessage("Status must be either 'draft', 'sent', 'paid', or 'cancelled'");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("Invoice must have at least one item");

            RuleFor(x => x.Subtotal)
                .GreaterThan(0)
                .PrecisionScale(18, 2, false);

            RuleFor(x => x.Tax)
                .GreaterThanOrEqualTo(0)
                .PrecisionScale(18, 2, false);

            RuleFor(x => x.Total)
                .GreaterThan(0)
                .PrecisionScale(18, 2, false)
                .Equal(x => x.Subtotal + x.Tax)
                .WithMessage("Total must be equal to Subtotal + Tax");
        }
    }
} 