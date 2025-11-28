using AppsWave.Ecommerce.Domain.Entities;
using FluentValidation;

namespace AppsWave.Ecommerce.Domain.Validators
{
    public class InvoiceDetailValidator : AbstractValidator<InvoiceDetail>
    {
        public InvoiceDetailValidator()
        {
            // InvoiceId can be 0 when creating new invoice (will be set after save)
            RuleFor(x => x.InvoiceId)
                .GreaterThanOrEqualTo(0).WithMessage("Invoice ID cannot be negative.");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be greater than zero.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
                .LessThanOrEqualTo(10000).WithMessage("Quantity must not exceed 10,000.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.")
                .LessThanOrEqualTo(999999.99m).WithMessage("Price must not exceed 999,999.99.");
        }
    }
}

