using AppsWave.Ecommerce.Domain.Entities;
using FluentValidation;

namespace AppsWave.Ecommerce.Domain.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.ArabicName)
                .NotEmpty().WithMessage("Arabic name is required.")
                .MaximumLength(200).WithMessage("Arabic name must not exceed 200 characters.");

            RuleFor(x => x.EnglishName)
                .NotEmpty().WithMessage("English name is required.")
                .MaximumLength(200).WithMessage("English name must not exceed 200 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.")
                .LessThanOrEqualTo(999999.99m).WithMessage("Price must not exceed 999,999.99.");
        }
    }
}

