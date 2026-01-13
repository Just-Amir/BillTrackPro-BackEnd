using BillTrackPro.Application.DTOs;
using BillTrackPro.Application.Validators;
using FluentAssertions;

namespace BillTrackPro.Tests.Validators;

public class CreateClientDtoValidatorTests
{
    private readonly CreateClientDtoValidator _validator;

    public CreateClientDtoValidatorTests()
    {
        _validator = new CreateClientDtoValidator();
    }

    [Fact]
    public void Validate_WithValidDto_ShouldPass()
    {
        // Arrange
        var dto = new CreateClientDto { Name = "John Doe", Email = "john@test.com", CompanyName = "Acme Inc" };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_WithEmptyName_ShouldFail(string? name)
    {
        // Arrange
        var dto = new CreateClientDto { Name = name!, Email = "john@test.com" };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name");
    }

    [Fact]
    public void Validate_WithShortName_ShouldFail()
    {
        // Arrange
        var dto = new CreateClientDto { Name = "A", Email = "john@test.com" };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Name" && e.ErrorMessage.Contains("2 characters"));
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("invalid@")]
    [InlineData("@invalid.com")]
    public void Validate_WithInvalidEmail_ShouldFail(string email)
    {
        // Arrange
        var dto = new CreateClientDto { Name = "John Doe", Email = email };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Email");
    }

    [Fact]
    public void Validate_WithEmptyEmail_ShouldFail()
    {
        // Arrange
        var dto = new CreateClientDto { Name = "John Doe", Email = "" };

        // Act
        var result = _validator.Validate(dto);

        // Assert
        result.IsValid.Should().BeFalse();
    }
}
