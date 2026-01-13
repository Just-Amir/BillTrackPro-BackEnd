# BillTrack Pro - Backend API

A professional REST API built with **.NET 10** following **Clean Architecture** principles.

![.NET](https://img.shields.io/badge/.NET-10.0-purple)
![EF Core](https://img.shields.io/badge/EF_Core-10.0-blue)
![xUnit](https://img.shields.io/badge/xUnit-2.9-green)

## 🏗️ Architecture (Clean Architecture)

```
Backend/
├── BillTrackPro.API/              # Presentation Layer
│   ├── Controllers/               # ClientsController, InvoicesController
│   └── Middleware/                # GlobalExceptionHandler
├── BillTrackPro.Application/      # Business Logic
│   ├── Common/                    # Result, ApiResponse patterns
│   ├── DTOs/                      # 7 Data Transfer Objects
│   ├── Services/                  # IClientService, IInvoiceService
│   └── Validators/                # 4 FluentValidation validators
├── BillTrackPro.Domain/           # Core Entities & Interfaces
│   ├── Entities/                  # Invoice, Client
│   └── Enums/                     # InvoiceStatus (Pending, Paid, Overdue)
├── BillTrackPro.Infrastructure/   # EF Core, Repositories
└── BillTrackPro.Tests/            # Unit Tests (xUnit + Moq)
    ├── Services/                  # ClientServiceTests, InvoiceServiceTests
    └── Validators/                # Dto Validator Tests
```

## 🚀 Getting Started

```bash
dotnet restore
dotnet ef database update --project BillTrackPro.Infrastructure --startup-project BillTrackPro.API
dotnet run --project BillTrackPro.API
```
API runs on `http://localhost:5251`

## 📋 API Endpoints

### Clients
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Clients` | Get all clients |
| GET | `/api/Clients/{id}` | Get client by ID |
| POST | `/api/Clients` | Create client |
| PUT | `/api/Clients/{id}` | Update client |
| DELETE | `/api/Clients/{id}` | Delete client |

### Invoices
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Invoices` | Get all invoices |
| GET | `/api/Invoices/{id}` | Get invoice by ID |
| GET | `/api/Invoices/dashboard-stats` | Dashboard statistics |
| POST | `/api/Invoices` | Create invoice |

## 🧪 Testing

```bash
dotnet test
```

| Test File | Tests | Coverage |
|-----------|-------|----------|
| `ClientServiceTests.cs` | 10 | CRUD Operations |
| `InvoiceServiceTests.cs` | 8 | CRUD + Stats |
| `CreateClientDtoValidatorTests.cs` | 5 | Validation |
| `CreateInvoiceDtoValidatorTests.cs` | 7 | Validation |
| **Total** | **30** | |

## 🛠️ Tech Stack

| Technology | Purpose |
|------------|---------|
| .NET 10 | Framework |
| Entity Framework Core | ORM |
| FluentValidation | Input Validation |
| xUnit + Moq + FluentAssertions | Testing |

## 📁 Key Patterns

### Result Pattern
```csharp
public async Task<Result<ClientDto>> GetClientByIdAsync(int id)
{
    var client = await _repository.GetByIdAsync(id);
    if (client == null)
        return Result<ClientDto>.Failure("Client not found");
    return Result<ClientDto>.Success(MapToDto(client));
}
```

### Global Exception Handler
Middleware catches all exceptions with typed `ErrorResponse`.

### FluentValidation
```csharp
RuleFor(x => x.Email)
    .NotEmpty().WithMessage("Email is required.")
    .EmailAddress().WithMessage("A valid email is required.");
```

## 📝 License: MIT
