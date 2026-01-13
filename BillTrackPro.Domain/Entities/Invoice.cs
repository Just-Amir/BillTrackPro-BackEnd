namespace BillTrackPro.Domain.Entities;

using BillTrackPro.Domain.Enums;

public class Invoice
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime DateIssued { get; set; }
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Pending; // Pending, Paid, Overdue
    
    public int ClientId { get; set; }
    public Client? Client { get; set; }
}
