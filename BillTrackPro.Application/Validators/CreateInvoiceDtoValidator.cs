using FluentValidation;
using BillTrackPro.Application.DTOs;

namespace BillTrackPro.Application.Validators;

public class CreateInvoiceDtoValidator : AbstractValidator<CreateInvoiceDto>
{
    public CreateInvoiceDtoValidator()
    {
        RuleFor(x => x.InvoiceNumber)
            .NotEmpty().WithMessage("Invoice Number is required.")
            .MaximumLength(50).WithMessage("Invoice Number must not exceed 50 characters.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.");

        RuleFor(x => x.ClientId)
            .GreaterThan(0).WithMessage("Valid Client ID is required.");

        RuleFor(x => x.Status)
            .Must(s => new[] { "Pending", "Paid", "Overdue" }.Contains(s))
            .WithMessage("Status must be one of: Pending, Paid, Overdue.");
    }
}
