using BillTrackPro.Application.Common;
using BillTrackPro.Application.DTOs;
using BillTrackPro.Application.Services;
using BillTrackPro.Domain.Entities;
using BillTrackPro.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace BillTrackPro.Tests.Services;

public class InvoiceServiceTests
{
    private readonly Mock<IInvoiceRepository> _mockInvoiceRepository;
    private readonly Mock<IRepository<Client>> _mockClientRepository;
    private readonly InvoiceService _sut;

    public InvoiceServiceTests()
    {
        _mockInvoiceRepository = new Mock<IInvoiceRepository>();
        _mockClientRepository = new Mock<IRepository<Client>>();
        _sut = new InvoiceService(_mockInvoiceRepository.Object, _mockClientRepository.Object);
    }

    #region GetAllInvoicesAsync Tests

    [Fact]
    public async Task GetAllInvoicesAsync_WhenInvoicesExist_ReturnsAllInvoices()
    {
        // Arrange
        var invoices = new List<Invoice>
        {
            CreateTestInvoice(1, "INV-001", 100, "Paid"),
            CreateTestInvoice(2, "INV-002", 200, "Pending")
        };
        _mockInvoiceRepository.Setup(r => r.GetInvoicesWithClientsAsync()).ReturnsAsync(invoices);

        // Act
        var result = await _sut.GetAllInvoicesAsync();

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        result.Data!.First().InvoiceNumber.Should().Be("INV-001");
    }

    [Fact]
    public async Task GetAllInvoicesAsync_WhenNoInvoices_ReturnsEmptyList()
    {
        // Arrange
        _mockInvoiceRepository.Setup(r => r.GetInvoicesWithClientsAsync()).ReturnsAsync(new List<Invoice>());

        // Act
        var result = await _sut.GetAllInvoicesAsync();

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().BeEmpty();
    }

    #endregion

    #region GetInvoiceByIdAsync Tests

    [Fact]
    public async Task GetInvoiceByIdAsync_WhenExists_ReturnsInvoice()
    {
        // Arrange
        var invoice = CreateTestInvoice(1, "INV-001", 150, "Paid");
        _mockInvoiceRepository.Setup(r => r.GetInvoiceWithClientByIdAsync(1)).ReturnsAsync(invoice);

        // Act
        var result = await _sut.GetInvoiceByIdAsync(1);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.InvoiceNumber.Should().Be("INV-001");
        result.Data.Amount.Should().Be(150);
    }

    [Fact]
    public async Task GetInvoiceByIdAsync_WhenNotFound_ReturnsFail()
    {
        // Arrange
        _mockInvoiceRepository.Setup(r => r.GetInvoiceWithClientByIdAsync(999)).ReturnsAsync((Invoice?)null);

        // Act
        var result = await _sut.GetInvoiceByIdAsync(999);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Contain("not found");
    }

    #endregion

    #region GetDashboardStatsAsync Tests

    [Fact]
    public async Task GetDashboardStatsAsync_CalculatesTotalRevenueCorrectly()
    {
        // Arrange
        var invoices = new List<Invoice>
        {
            CreateTestInvoice(1, "INV-001", 1000, "Paid"),
            CreateTestInvoice(2, "INV-002", 500, "Paid"),
            CreateTestInvoice(3, "INV-003", 250, "Pending")
        };
        _mockInvoiceRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(invoices);

        // Act
        var result = await _sut.GetDashboardStatsAsync();

        // Assert
        result.Success.Should().BeTrue();
        result.Data!.TotalRevenue.RawValue.Should().Be(1500); // 1000 + 500 (Paid only)
    }

    [Fact]
    public async Task GetDashboardStatsAsync_CalculatesOutstandingCorrectly()
    {
        // Arrange
        var invoices = new List<Invoice>
        {
            CreateTestInvoice(1, "INV-001", 1000, "Paid"),
            CreateTestInvoice(2, "INV-002", 500, "Pending"),
            CreateTestInvoice(3, "INV-003", 250, "Overdue")
        };
        _mockInvoiceRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(invoices);

        // Act
        var result = await _sut.GetDashboardStatsAsync();

        // Assert
        result.Success.Should().BeTrue();
        result.Data!.Outstanding.RawValue.Should().Be(750); // Pending + Overdue
    }

    #endregion

    #region CreateInvoiceAsync Tests

    [Fact]
    public async Task CreateInvoiceAsync_WithValidDto_CreatesInvoice()
    {
        // Arrange
        var dto = new CreateInvoiceDto 
        { 
            InvoiceNumber = "INV-NEW", 
            Amount = 500, 
            ClientId = 1, 
            Status = "Pending",
            DateIssued = DateTime.UtcNow
        };
        
        // Mock AddAsync
        _mockInvoiceRepository.Setup(r => r.AddAsync(It.IsAny<Invoice>())).Returns(Task.CompletedTask);
        
        // Mock GetInvoiceWithClientByIdAsync to return the created invoice (logic inside service)
        var createdInvoice = CreateTestInvoice(1, "INV-NEW", 500, "Pending");
        _mockInvoiceRepository.Setup(r => r.GetInvoiceWithClientByIdAsync(It.IsAny<int>())).ReturnsAsync(createdInvoice);

        // Act
        var result = await _sut.CreateInvoiceAsync(dto);

        // Assert
        result.Success.Should().BeTrue();
        result.Data!.InvoiceNumber.Should().Be("INV-NEW");
        _mockInvoiceRepository.Verify(r => r.AddAsync(It.IsAny<Invoice>()), Times.Once);
    }

    #endregion

    #region Helper Methods

    private static Invoice CreateTestInvoice(int id, string number, decimal amount, string status)
    {
        return new Invoice
        {
            Id = id,
            InvoiceNumber = number,
            Amount = amount,
            Status = status,
            DateIssued = DateTime.UtcNow,
            ClientId = 1,
            Client = new Client 
            { 
                Id = 1, 
                Name = "Test Client", 
                Email = "test@test.com",
                CompanyName = "Test Co",
                IsActive = true,
                AvatarUrl = ""
            }
        };
    }

    #endregion
}
