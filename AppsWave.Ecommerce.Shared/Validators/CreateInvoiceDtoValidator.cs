using AppsWave.Ecommerce.Shared.DTOs;
using FluentValidation;

namespace AppsWave.Ecommerce.Shared.Validators
{
    public class CreateInvoiceDtoValidator : AbstractValidator<CreateInvoiceDto>
    {
        public CreateInvoiceDtoValidator()
        {
            RuleFor(x => x.Details)
                .NotEmpty().WithMessage("Invoice must have at least one item.")
                .Must(details => details.Count > 0).WithMessage("Invoice must have at least one item.");

            RuleForEach(x => x.Details)
                .SetValidator(new CreateInvoiceDetailDtoValidator());
        }
    }

    public class CreateInvoiceDetailDtoValidator : AbstractValidator<CreateInvoiceDetailDto>
    {
        public CreateInvoiceDetailDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be greater than zero.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}

