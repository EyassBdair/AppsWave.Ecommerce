using AppsWave.Ecommerce.Domain.Entities;
using FluentValidation;

namespace AppsWave.Ecommerce.Domain.Validators
{
    public class InvoiceValidator : AbstractValidator<Invoice>
    {
        public InvoiceValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID must be greater than zero.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date cannot be in the future.");

            RuleFor(x => x.TotalAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Total amount cannot be negative.");

            RuleFor(x => x.Details)
                .NotEmpty().WithMessage("Invoice must have at least one detail.")
                .Must(details => details != null && details.Count > 0).WithMessage("Invoice must have at least one detail.");

            RuleForEach(x => x.Details)
                .SetValidator(new InvoiceDetailValidator());
        }
    }
}

