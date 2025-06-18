using FluentValidation;
using FimwiApi.Core.Entities;

namespace FimwiApi.Core.Validators
{
    public class BatchValidator : AbstractValidator<Batch>
    {
        public BatchValidator()
        {
            RuleFor(x => x.BatchNumber)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.Quantity)
                .GreaterThan(0);

            RuleFor(x => x.PurchasePrice)
                .GreaterThan(0)
                .PrecisionScale(18, 2, false);

            RuleFor(x => x.ManufacturingDate)
                .NotEmpty()
                .LessThan(x => x.ExpirationDate)
                .WithMessage("ManufacturingDate must be less than ExpirationDate");

            RuleFor(x => x.ExpirationDate)
                .NotEmpty()
                .GreaterThan(x => x.ManufacturingDate)
                .WithMessage("ExpirationDate must be greater than ManufacturingDate");

            RuleFor(x => x.Status)
                .NotEmpty()
                .Must(status => status == "active" || status == "expired" || status == "depleted")
                .WithMessage("Status must be either 'active', 'expired', or 'depleted'");
        }
    }
} 