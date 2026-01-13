using BillTrackPro.Domain.Entities;
using BillTrackPro.Domain.Enums;
using BillTrackPro.Domain.Interfaces;
using BillTrackPro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BillTrackPro.Infrastructure.Repositories;

public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(BillTrackDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Invoice>> GetInvoicesWithClientsAsync()
    {
        return await _context.Invoices
            .Include(i => i.Client)
            .OrderByDescending(i => i.DateIssued)
            .ToListAsync();
    }

    public async Task<Invoice?> GetInvoiceWithClientByIdAsync(int id)
    {
        return await _context.Invoices
            .Include(i => i.Client)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<decimal> GetTotalRevenueAsync()
    {
        return await _context.Invoices
            .Where(i => i.Status == InvoiceStatus.Paid)
            .SumAsync(i => i.Amount);
    }
}
