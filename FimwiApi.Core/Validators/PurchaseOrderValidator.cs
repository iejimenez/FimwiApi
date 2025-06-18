using FluentValidation;
using FimwiApi.Core.Entities;

namespace FimwiApi.Core.Validators
{
    public class PurchaseOrderValidator : AbstractValidator<PurchaseOrder>
    {
        public PurchaseOrderValidator()
        {
            RuleFor(x => x.OrderNumber)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.Date)
                .NotEmpty();

            RuleFor(x => x.Status)
                .NotEmpty()
                .Must(status => status == "draft" || status == "pending" || status == "approved" || status == "received" || status == "cancelled")
                .WithMessage("Status must be either 'draft', 'pending', 'approved', 'received', or 'cancelled'");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("Purchase order must have at least one item");

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