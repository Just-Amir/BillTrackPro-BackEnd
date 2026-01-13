using FluentValidation;
using BillTrackPro.Application.DTOs;

namespace BillTrackPro.Application.Validators;

public class UpdateInvoiceDtoValidator : AbstractValidator<UpdateInvoiceDto>
{
    private static readonly string[] ValidStatuses = { "Pending", "Paid", "Overdue" };

    public UpdateInvoiceDtoValidator()
    {
        RuleFor(x => x.InvoiceNumber)
            .MaximumLength(50).WithMessage("Invoice Number must not exceed 50 characters.")
            .When(x => !string.IsNullOrEmpty(x.InvoiceNumber));

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0.")
            .When(x => x.Amount.HasValue);

        RuleFor(x => x.Status)
            .Must(s => ValidStatuses.Contains(s))
            .WithMessage("Status must be one of: Pending, Paid, Overdue.")
            .When(x => !string.IsNullOrEmpty(x.Status));

        RuleFor(x => x.DateIssued)
            .LessThanOrEqualTo(DateTime.UtcNow.AddDays(1))
            .WithMessage("Date issued cannot be in the future.")
            .When(x => x.DateIssued.HasValue);
    }
}
