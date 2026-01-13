using BillTrackPro.Application.Services;
using BillTrackPro.Application.DTOs;
using BillTrackPro.Domain.Entities;
using BillTrackPro.Domain.Interfaces;
using Moq;
using FluentAssertions;

namespace BillTrackPro.Tests.Services;

public class ClientServiceTests
{
    private readonly Mock<IRepository<Client>> _mockRepository;
    private readonly ClientService _sut; // System Under Test

    public ClientServiceTests()
    {
        _mockRepository = new Mock<IRepository<Client>>();
        _sut = new ClientService(_mockRepository.Object);
    }

    #region GetAllClientsAsync Tests

    [Fact]
    public async Task GetAllClientsAsync_WhenClientsExist_ReturnsSuccessWithClients()
    {
        // Arrange
        var clients = new List<Client>
        {
            new() { Id = 1, Name = "John Doe", Email = "john@test.com", CompanyName = "Acme", IsActive = true, Invoices = new List<Invoice>() },
            new() { Id = 2, Name = "Jane Doe", Email = "jane@test.com", CompanyName = "Corp", IsActive = true, Invoices = new List<Invoice>() }
        };
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(clients);

        // Act
        var result = await _sut.GetAllClientsAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        result.Value!.First().Name.Should().Be("John Doe");
    }

    [Fact]
    public async Task GetAllClientsAsync_WhenNoClients_ReturnsEmptyList()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Client>());

        // Act
        var result = await _sut.GetAllClientsAsync();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAllClientsAsync_CalculatesLifetimeValueCorrectly()
    {
        // Arrange
        var client = new Client
        {
            Id = 1, Name = "Test", Email = "test@test.com", CompanyName = "Test Co", IsActive = true,
            Invoices = new List<Invoice>
            {
                new() { Id = 1, Amount = 100, Status = "Paid" },
                new() { Id = 2, Amount = 200, Status = "Paid" }
            }
        };
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Client> { client });

        // Act
        var result = await _sut.GetAllClientsAsync();

        // Assert
        result.Value!.First().LifetimeValue.Should().Be(300);
    }

    [Fact]
    public async Task GetAllClientsAsync_SetsOutstandingStatusToOverdue_WhenHasOverdueInvoices()
    {
        // Arrange
        var client = new Client
        {
            Id = 1, Name = "Test", Email = "test@test.com", CompanyName = "Test Co", IsActive = true,
            Invoices = new List<Invoice>
            {
                new() { Id = 1, Amount = 100, Status = "Overdue" }
            }
        };
        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Client> { client });

        // Act
        var result = await _sut.GetAllClientsAsync();

        // Assert
        result.Value!.First().OutstandingStatus.Should().Be("Overdue");
    }

    #endregion

    #region GetClientByIdAsync Tests

    [Fact]
    public async Task GetClientByIdAsync_WhenClientExists_ReturnsSuccess()
    {
        // Arrange
        var client = new Client { Id = 1, Name = "John", Email = "john@test.com", CompanyName = "Acme", IsActive = true, Invoices = new List<Invoice>() };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(client);

        // Act
        var result = await _sut.GetClientByIdAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Name.Should().Be("John");
    }

    [Fact]
    public async Task GetClientByIdAsync_WhenClientNotFound_ReturnsFailure()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Client?)null);

        // Act
        var result = await _sut.GetClientByIdAsync(999);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("not found");
    }

    #endregion

    #region CreateClientAsync Tests

    [Fact]
    public async Task CreateClientAsync_WithValidDto_ReturnsSuccess()
    {
        // Arrange
        var dto = new CreateClientDto { Name = "New Client", Email = "new@test.com", CompanyName = "New Co" };
        _mockRepository.Setup(r => r.AddAsync(It.IsAny<Client>())).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.CreateClientAsync(dto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Name.Should().Be("New Client");
        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Client>()), Times.Once);
    }

    #endregion

    #region UpdateClientAsync Tests

    [Fact]
    public async Task UpdateClientAsync_WhenClientExists_UpdatesAndReturnsSuccess()
    {
        // Arrange
        var existingClient = new Client { Id = 1, Name = "Old Name", Email = "old@test.com", CompanyName = "Old Co", IsActive = true, Invoices = new List<Invoice>() };
        var updateDto = new UpdateClientDto { Name = "New Name" };
        
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingClient);
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Client>())).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.UpdateClientAsync(1, updateDto);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.Name.Should().Be("New Name");
    }

    [Fact]
    public async Task UpdateClientAsync_WhenClientNotFound_ReturnsFailure()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Client?)null);

        // Act
        var result = await _sut.UpdateClientAsync(999, new UpdateClientDto { Name = "Test" });

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    #endregion

    #region DeleteClientAsync Tests

    [Fact]
    public async Task DeleteClientAsync_WhenClientExists_ReturnsSuccess()
    {
        // Arrange
        var client = new Client { Id = 1, Name = "Test", Email = "test@test.com", CompanyName = "Co", IsActive = true, Invoices = new List<Invoice>() };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(client);
        _mockRepository.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.DeleteClientAsync(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteClientAsync_WhenClientNotFound_ReturnsFailure()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync(999)).ReturnsAsync((Client?)null);

        // Act
        var result = await _sut.DeleteClientAsync(999);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    #endregion
}
