using AppsWave.Ecommerce.Shared.DTOs;
using FluentValidation;

namespace AppsWave.Ecommerce.Shared.Validators
{
    public class ProductDtoValidator : AbstractValidator<ProductDto>
    {
        public ProductDtoValidator()
        {
            RuleFor(x => x.ArabicName)
                .NotEmpty().WithMessage("Arabic name is required.")
                .MaximumLength(200).WithMessage("Arabic name must not exceed 200 characters.");

            RuleFor(x => x.EnglishName)
                .NotEmpty().WithMessage("English name is required.")
                .MaximumLength(200).WithMessage("English name must not exceed 200 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");
        }
    }
}

