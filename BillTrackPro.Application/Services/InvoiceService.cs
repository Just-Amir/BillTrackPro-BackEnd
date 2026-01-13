using BillTrackPro.Application.Common;
using BillTrackPro.Application.DTOs;
using BillTrackPro.Domain.Common;
using BillTrackPro.Domain.Entities;
using BillTrackPro.Domain.Interfaces;
using System.Linq.Expressions;

namespace BillTrackPro.Application.Services;

using BillTrackPro.Domain.Enums;

public interface IInvoiceService
{
    Task<ServiceResponse<PagedResult<InvoiceDto>>> GetPagedInvoicesAsync(int page, int pageSize, string? search, string? status, string? orderBy = null, bool isDescending = false);
    Task<ServiceResponse<InvoiceDto>> GetInvoiceByIdAsync(int id);
    Task<ServiceResponse<InvoiceDto>> CreateInvoiceAsync(CreateInvoiceDto invoiceDto);
    Task<ServiceResponse<bool>> UpdateInvoiceAsync(int id, CreateInvoiceDto invoiceDto);
    Task<ServiceResponse<bool>> DeleteInvoiceAsync(int id);
    Task<ServiceResponse<DashboardStatsDto>> GetDashboardStatsAsync();
    Task<ServiceResponse<ReportsDto>> GetReportsDataAsync();
}

public class InvoiceService : IInvoiceService
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IRepository<Client> _clientRepository;

    public InvoiceService(IInvoiceRepository invoiceRepository, IRepository<Client> clientRepository)
    {
        _invoiceRepository = invoiceRepository;
        _clientRepository = clientRepository;
    }

    public async Task<ServiceResponse<PagedResult<InvoiceDto>>> GetPagedInvoicesAsync(int page, int pageSize, string? search, string? status, string? orderBy = null, bool isDescending = false)
    {
        Expression<Func<Invoice, bool>>? predicate = null;

        if (!string.IsNullOrWhiteSpace(search) || !string.IsNullOrWhiteSpace(status))
        {
            var term = search?.Trim().ToLower();
            
            predicate = i => 
                (string.IsNullOrEmpty(term) || (i.InvoiceNumber.ToLower().Contains(term) || (i.Client != null && i.Client.Name.ToLower().Contains(term)))) &&
                (string.IsNullOrEmpty(status) || status == "All" || i.Status.ToString() == status);
        }

        var pagedResult = await _invoiceRepository.GetPagedAsync(page, pageSize, predicate, orderBy, isDescending, i => i.Client!);
        var dtos = pagedResult.Items.Select(MapToDto);
        
        var result = new PagedResult<InvoiceDto>(dtos, pagedResult.TotalCount, pagedResult.PageNumber, pagedResult.PageSize);
        return ServiceResponse<PagedResult<InvoiceDto>>.Ok(result);
    }

    public async Task<ServiceResponse<InvoiceDto>> GetInvoiceByIdAsync(int id)
    {
        var invoice = await _invoiceRepository.GetInvoiceWithClientByIdAsync(id);
        if (invoice == null) 
            return ServiceResponse<InvoiceDto>.Fail("Invoice not found");
            
        return ServiceResponse<InvoiceDto>.Ok(MapToDto(invoice));
    }

    public async Task<ServiceResponse<InvoiceDto>> CreateInvoiceAsync(CreateInvoiceDto dto)
    {
        var invoice = new Invoice
        {
            InvoiceNumber = dto.InvoiceNumber,
            Amount = dto.Amount,
            DateIssued = dto.DateIssued,
            Status = Enum.Parse<InvoiceStatus>(dto.Status),
            ClientId = dto.ClientId
        };

        await _invoiceRepository.AddAsync(invoice);
        
        var createdInvoice = await _invoiceRepository.GetInvoiceWithClientByIdAsync(invoice.Id);
        if (createdInvoice == null)
             return ServiceResponse<InvoiceDto>.Fail("Failed to retrieve created invoice");
        
        return ServiceResponse<InvoiceDto>.Ok(MapToDto(createdInvoice), "Invoice created successfully");
    }

    public async Task<ServiceResponse<bool>> UpdateInvoiceAsync(int id, CreateInvoiceDto dto)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id);
        if (invoice == null) 
             return ServiceResponse<bool>.Fail("Invoice not found");

        invoice.InvoiceNumber = dto.InvoiceNumber;
        invoice.Amount = dto.Amount;
        invoice.DateIssued = dto.DateIssued;
        invoice.Status = Enum.Parse<InvoiceStatus>(dto.Status);
        invoice.ClientId = dto.ClientId;

        await _invoiceRepository.UpdateAsync(invoice);
        return ServiceResponse<bool>.Ok(true, "Invoice updated");
    }

    public async Task<ServiceResponse<bool>> DeleteInvoiceAsync(int id)
    {
        var invoice = await _invoiceRepository.GetByIdAsync(id);
        if (invoice == null)
            return ServiceResponse<bool>.Fail("Invoice not found");

        await _invoiceRepository.DeleteAsync(id);
        return ServiceResponse<bool>.Ok(true, "Invoice deleted");
    }

    public async Task<ServiceResponse<DashboardStatsDto>> GetDashboardStatsAsync()
    {
        var allInvoices = await _invoiceRepository.GetAllAsync();
        var invoices = allInvoices.ToList(); 

        var now = DateTime.UtcNow;
        var thisMonth = new DateTime(now.Year, now.Month, 1);
        var lastMonth = thisMonth.AddMonths(-1);

        // 1. Total Revenue (Paid Invoices)
        var revenueThisMonth = invoices
            .Where(i => i.Status == InvoiceStatus.Paid && i.DateIssued >= thisMonth)
            .Sum(i => i.Amount);
        
        var revenueLastMonth = invoices
            .Where(i => i.Status == InvoiceStatus.Paid && i.DateIssued >= lastMonth && i.DateIssued < thisMonth)
            .Sum(i => i.Amount);

        var totalRevenue = invoices.Where(i => i.Status == InvoiceStatus.Paid).Sum(i => i.Amount);

        // Calculate Revenue Growth %
        double revenueGrowth = 0;
        if (revenueLastMonth > 0)
        {
            revenueGrowth = (double)((revenueThisMonth - revenueLastMonth) / revenueLastMonth) * 100;
        }
        else if (revenueThisMonth > 0)
        {
            revenueGrowth = 100;
        }

        // 2. Outstanding (Pending + Overdue)
        var outstanding = invoices
            .Where(i => i.Status == InvoiceStatus.Pending || i.Status == InvoiceStatus.Overdue)
            .Sum(i => i.Amount);
        
        // Mocking previous outstanding for comparison demo
        var outstandingLastMonth = outstanding * 0.9m; 
        double outstandingGrowth = 2.4; 

        // 3. Monthly Growth (Total Invoiced This Month vs Last Month)
        var invoicedThisMonth = invoices.Where(i => i.DateIssued >= thisMonth).Sum(i => i.Amount);
        var invoicedLastMonth = invoices.Where(i => i.DateIssued >= lastMonth && i.DateIssued < thisMonth).Sum(i => i.Amount);
        
        double monthlyGrowth = 0;
        if (invoicedLastMonth > 0)
             monthlyGrowth = (double)((invoicedThisMonth - invoicedLastMonth) / invoicedLastMonth) * 100;


        var stats = new DashboardStatsDto
        {
            TotalRevenue = new MetricItem 
            { 
                RawValue = totalRevenue, 
                ValueFormatted = totalRevenue.ToString("C"), 
                ChangePercentage = $"{revenueGrowth:0.0}%",
                IsPositive = revenueGrowth >= 0,
                Trend = Enumerable.Range(0, 6)
                        .Select(i => 
                        {
                             var d = now.AddMonths(- (5 - i));
                             var ms = new DateTime(d.Year, d.Month, 1);
                             var me = ms.AddMonths(1);
                             return invoices.Where(inv => inv.Status == InvoiceStatus.Paid && inv.DateIssued >= ms && inv.DateIssued < me).Sum(inv => inv.Amount);
                        }).ToList()
            },
            Outstanding = new MetricItem 
            { 
                RawValue = outstanding, 
                ValueFormatted = outstanding.ToString("C"), 
                ChangePercentage = $"{outstandingGrowth:0.0}%", 
                IsPositive = false,
                Trend = Enumerable.Range(0, 6)
                        .Select(i => 
                        {
                             var d = now.AddMonths(- (5 - i));
                             var ms = new DateTime(d.Year, d.Month, 1);
                             var me = ms.AddMonths(1);
                             return invoices.Where(inv => (inv.Status == InvoiceStatus.Pending || inv.Status == InvoiceStatus.Overdue) && inv.DateIssued >= ms && inv.DateIssued < me).Sum(inv => inv.Amount);
                        }).ToList() 
            },
            MonthlyGrowth = new MetricItem
            {
                RawValue = (decimal)monthlyGrowth,
                ValueFormatted = $"{monthlyGrowth:0.0}%",
                ChangePercentage = $"{monthlyGrowth:0.0}%", 
                IsPositive = monthlyGrowth >= 0,
                Trend = Enumerable.Range(0, 6).Select(_ => (decimal)new Random().NextDouble() * 100).ToList() // Mock trend for growth %
            }
        };

        return ServiceResponse<DashboardStatsDto>.Ok(stats);
    }

    public async Task<ServiceResponse<ReportsDto>> GetReportsDataAsync()
    {
        var allInvoices = await _invoiceRepository.GetAllAsync();
        var allClients = await _clientRepository.GetAllAsync();

        var invoices = allInvoices.ToList();
        var clients = allClients.ToList();

        // 1. Total Revenue & Expenses
        var totalRevenue = invoices.Where(i => i.Status == InvoiceStatus.Paid).Sum(i => i.Amount);
        var totalExpenses = totalRevenue * 0.6m; // Mock logic for expenses

        // 2. Monthly Revenue (Last 12 Months)
        var monthlyRevenue = new List<MonthlyRevenueDto>();
        var now = DateTime.UtcNow;
        for (int i = 11; i >= 0; i--)
        {
            var date = now.AddMonths(-i);
            var monthStart = new DateTime(date.Year, date.Month, 1);
            var monthEnd = monthStart.AddMonths(1);
            
            var revenue = invoices
                .Where(inv => inv.Status == InvoiceStatus.Paid && inv.DateIssued >= monthStart && inv.DateIssued < monthEnd)
                .Sum(inv => inv.Amount);

            monthlyRevenue.Add(new MonthlyRevenueDto
            {
                Month = date.ToString("MMM"), // Jan, Feb...
                Revenue = revenue,
                Expenses = revenue * 0.6m
            });
        }

        // 3. Revenue By Client
        var revenueByClient = clients.Select(c => 
        {
            var clientRevenue = invoices.Where(inv => inv.ClientId == c.Id && inv.Status == InvoiceStatus.Paid).Sum(inv => inv.Amount);
            return new ClientRevenueDto
            {
                Name = c.Name,
                Value = clientRevenue,
                Percentage = totalRevenue > 0 ? (double)(clientRevenue / totalRevenue * 100) : 0
            };
        })
        .OrderByDescending(x => x.Value)
        .Take(5)
        .ToList();

        // 4. Invoices By Status
        var invoicesByStatus = invoices
            .GroupBy(i => i.Status)
            .Select(g => new StatusCountDto
            {
                Status = g.Key.ToString(),
                Count = g.Count()
            })
            .ToList();

        var report = new ReportsDto
        {
            TotalRevenue = totalRevenue,
            TotalExpenses = totalExpenses,
            MonthlyRevenue = monthlyRevenue,
            RevenueByClient = revenueByClient,
            InvoicesByStatus = invoicesByStatus
        };

        return ServiceResponse<ReportsDto>.Ok(report);
    }

    private static InvoiceDto MapToDto(Invoice invoice)
    {
        return new InvoiceDto
        {
            Id = invoice.Id,
            InvoiceNumber = invoice.InvoiceNumber,
            Amount = invoice.Amount,
            DateIssued = invoice.DateIssued,
            Status = invoice.Status.ToString(),
            ClientId = invoice.ClientId,
            ClientName = invoice.Client?.Name ?? "Unknown",
            ClientEmail = invoice.Client?.Email ?? "",
            ClientAvatarUrl = invoice.Client?.AvatarUrl ?? ""
        };
    }
}
