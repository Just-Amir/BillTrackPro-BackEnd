namespace BillTrackPro.Application.DTOs;

// ============================================
// Client DTOs
// ============================================

public class ClientDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public decimal LifetimeValue { get; set; }
    public string OutstandingStatus { get; set; } = "All Paid";
}

public class CreateClientDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
}

public class UpdateClientDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? CompanyName { get; set; }
    public bool? IsActive { get; set; }
}

// ============================================
// Invoice DTOs
// ============================================

public class InvoiceDto
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime DateIssued { get; set; }
    public string Status { get; set; } = string.Empty;
    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string ClientEmail { get; set; } = string.Empty;
    public string ClientAvatarUrl { get; set; } = string.Empty;
}

public class CreateInvoiceDto
{
    public string InvoiceNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime DateIssued { get; set; }
    public string Status { get; set; } = "Pending";
    public int ClientId { get; set; }
}

public class UpdateInvoiceDto
{
    public string? InvoiceNumber { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? DateIssued { get; set; }
    public string? Status { get; set; }
}

// ============================================
// Dashboard DTOs
// ============================================

public class DashboardStatsDto
{
    public MetricItem TotalRevenue { get; set; } = new();
    public MetricItem Outstanding { get; set; } = new();
    public MetricItem MonthlyGrowth { get; set; } = new();
}

public class MetricItem
{
    public string ValueFormatted { get; set; } = string.Empty;
    public decimal RawValue { get; set; }
    public string ChangePercentage { get; set; } = string.Empty;
    public bool IsPositive { get; set; }
    public string ComparisonText { get; set; } = "vs last month";
    public List<decimal> Trend { get; set; } = new();
}
