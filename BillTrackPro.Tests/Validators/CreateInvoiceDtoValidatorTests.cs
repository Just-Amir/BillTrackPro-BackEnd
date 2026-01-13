using BillTrackPro.Application.DTOs;
using BillTrackPro.Application.Validators;
using FluentAssertions;

namespace BillTrackPro.Tests.Validators;

public class CreateInvoiceDtoValidatorTests
{
    private readonly CreateInvoiceDtoValidator _validator;

    public CreateInvoiceDtoValidatorTests()
    {
        _validator = new CreateInvoiceDtoValidator();
    }

    [Fact]
    public void Validate_WithValidDto_ShouldPass()
    {
        // Arrange
        var dto = new CreateInvoiceDto 
        { 
            InvoiceNumber = "INV-001", 
            Amount = 100.50m, 
            ClientId = 1, 
            Status = "Pending",
            DateIssued = DateTime.UtcNow
        };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_WithEmptyInvoiceNumber_ShouldFail(string? invoiceNumber)
    {
        // Arrange
        var dto = new CreateInvoiceDto { InvoiceNumber = invoiceNumber!, Amount = 100, ClientId = 1, Status = "Pending" };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "InvoiceNumber");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Validate_WithInvalidAmount_ShouldFail(decimal amount)
    {
        // Arrange
        var dto = new CreateInvoiceDto { InvoiceNumber = "INV-001", Amount = amount, ClientId = 1, Status = "Pending" };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Amount");
    }

    [Theory]
    [InlineData("Invalid")]
    [InlineData("Completed")]
    [InlineData("")]
    public void Validate_WithInvalidStatus_ShouldFail(string status)
    {
        // Arrange
        var dto = new CreateInvoiceDto { InvoiceNumber = "INV-001", Amount = 100, ClientId = 1, Status = status };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Status");
    }

    [Theory]
    [InlineData("Pending")]
    [InlineData("Paid")]
    [InlineData("Overdue")]
    public void Validate_WithValidStatus_ShouldPass(string status)
    {
        // Arrange
        var dto = new CreateInvoiceDto { InvoiceNumber = "INV-001", Amount = 100, ClientId = 1, Status = status };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WithInvalidClientId_ShouldFail(int clientId)
    {
        // Arrange
        var dto = new CreateInvoiceDto { InvoiceNumber = "INV-001", Amount = 100, ClientId = clientId, Status = "Pending" };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "ClientId");
    }
}
