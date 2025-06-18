using FluentValidation;
using FimwiApi.Core.Entities;

namespace FimwiApi.Core.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Sku)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(500);

            RuleFor(x => x.Category)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0)
                .PrecisionScale(18, 2, false);

            RuleFor(x => x.MinStock)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.MaxStock)
                .GreaterThanOrEqualTo(0)
                .GreaterThanOrEqualTo(x => x.MinStock)
                .WithMessage("MaxStock must be greater than or equal to MinStock");

            RuleFor(x => x.CurrentStock)
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(x => x.MaxStock)
                .WithMessage("CurrentStock must be less than or equal to MaxStock");
        }
    }
} 