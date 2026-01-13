using BillTrackPro.Domain.Entities;

namespace BillTrackPro.Domain.Interfaces;

public interface IInvoiceRepository : IRepository<Invoice>
{
    Task<IEnumerable<Invoice>> GetInvoicesWithClientsAsync();
    Task<Invoice?> GetInvoiceWithClientByIdAsync(int id);
    Task<decimal> GetTotalRevenueAsync();
}
