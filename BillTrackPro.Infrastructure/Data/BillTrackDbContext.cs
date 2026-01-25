using BillTrackPro.Domain.Entities;
using BillTrackPro.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BillTrackPro.Infrastructure.Data;

public class BillTrackDbContext : DbContext
{
    public BillTrackDbContext(DbContextOptions<BillTrackDbContext> options) : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("BillTrack");

        modelBuilder.Entity<Invoice>()
            .Property(i => i.Amount)
            .HasColumnType("decimal(18,2)");
            
        // Seeding Data
        modelBuilder.Entity<Client>().HasData(
            new Client { Id = 1, Name = "Apex Logistics LLC", Email = "contact@apexlogistics.com", AvatarUrl = "https://i.pravatar.cc/150?u=1", CompanyName = "Apex Logistics", IsActive = true },
            new Client { Id = 2, Name = "Globex Inc", Email = "info@globex.com", AvatarUrl = "https://i.pravatar.cc/150?u=2", CompanyName = "Globex Corporation", IsActive = true },
            new Client { Id = 3, Name = "Soylent Corp", Email = "sales@soylent.com", AvatarUrl = "https://i.pravatar.cc/150?u=3", CompanyName = "Soylent Corp", IsActive = false },
            new Client { Id = 4, Name = "Initech", Email = "bill@initech.com", AvatarUrl = "https://i.pravatar.cc/150?u=4", CompanyName = "Initech Systems", IsActive = true },
            new Client { Id = 5, Name = "Umbrella Corp", Email = "alice@umbrella.com", AvatarUrl = "https://i.pravatar.cc/150?u=5", CompanyName = "Umbrella Corp", IsActive = true },
            new Client { Id = 6, Name = "Stark Industries", Email = "tony@stark.com", AvatarUrl = "https://i.pravatar.cc/150?u=6", CompanyName = "Stark Industries", IsActive = true },
            new Client { Id = 7, Name = "Cyberdyne Systems", Email = "skynet@cyberdyne.com", AvatarUrl = "https://i.pravatar.cc/150?u=7", CompanyName = "Cyberdyne", IsActive = true },
            new Client { Id = 8, Name = "Massive Dynamic", Email = "nina@massive.com", AvatarUrl = "https://i.pravatar.cc/150?u=8", CompanyName = "Massive Dynamic", IsActive = true },
            new Client { Id = 9, Name = "Hooli", Email = "gavin@hooli.com", AvatarUrl = "https://i.pravatar.cc/150?u=9", CompanyName = "Hooli XYZ", IsActive = false },
            new Client { Id = 10, Name = "Pied Piper", Email = "richard@piedpiper.com", AvatarUrl = "https://i.pravatar.cc/150?u=10", CompanyName = "Pied Piper", IsActive = true },
            new Client { Id = 11, Name = "Wayne Enterprises", Email = "bruce@wayne.com", AvatarUrl = "https://i.pravatar.cc/150?u=11", CompanyName = "Wayne Enterprises", IsActive = true },
            new Client { Id = 12, Name = "Acme Corp", Email = "roadrunner@acme.com", AvatarUrl = "https://i.pravatar.cc/150?u=12", CompanyName = "Acme Corp", IsActive = true },
            new Client { Id = 13, Name = "Oscorp", Email = "norman@oscorp.com", AvatarUrl = "https://i.pravatar.cc/150?u=13", CompanyName = "Oscorp Industries", IsActive = true },
            new Client { Id = 14, Name = "Nakatomi Trading", Email = "takagi@nakatomi.com", AvatarUrl = "https://i.pravatar.cc/150?u=14", CompanyName = "Nakatomi", IsActive = true },
            new Client { Id = 15, Name = "Tyrell Corp", Email = "elden@tyrell.com", AvatarUrl = "https://i.pravatar.cc/150?u=15", CompanyName = "Tyrell Corporation", IsActive = false }
        );

        modelBuilder.Entity<Invoice>().HasData(
            // Recent invoices spread across last 12 months for chart data
            new Invoice { Id = 1, InvoiceNumber = "INV-2026-001", ClientId = 1, Amount = 12450.32m, DateIssued = DateTime.Parse("2026-01-10"), Status = InvoiceStatus.Paid },
            new Invoice { Id = 2, InvoiceNumber = "INV-2026-002", ClientId = 2, Amount = 3450.00m, DateIssued = DateTime.Parse("2026-01-05"), Status = InvoiceStatus.Pending },
            new Invoice { Id = 3, InvoiceNumber = "INV-2025-003", ClientId = 3, Amount = 8900.00m, DateIssued = DateTime.Parse("2025-12-15"), Status = InvoiceStatus.Paid },
            new Invoice { Id = 4, InvoiceNumber = "INV-2025-004", ClientId = 4, Amount = 2100.00m, DateIssued = DateTime.Parse("2025-12-01"), Status = InvoiceStatus.Paid },
            new Invoice { Id = 5, InvoiceNumber = "INV-2025-005", ClientId = 5, Amount = 5600.00m, DateIssued = DateTime.Parse("2025-11-20"), Status = InvoiceStatus.Overdue },
            new Invoice { Id = 6, InvoiceNumber = "INV-2025-006", ClientId = 6, Amount = 15420.00m, DateIssued = DateTime.Parse("2025-11-10"), Status = InvoiceStatus.Paid },
            new Invoice { Id = 7, InvoiceNumber = "INV-2025-007", ClientId = 7, Amount = 4500.00m, DateIssued = DateTime.Parse("2025-10-25"), Status = InvoiceStatus.Paid },
            new Invoice { Id = 8, InvoiceNumber = "INV-2025-008", ClientId = 8, Amount = 7200.00m, DateIssued = DateTime.Parse("2025-10-15"), Status = InvoiceStatus.Paid },
            new Invoice { Id = 9, InvoiceNumber = "INV-2025-009", ClientId = 1, Amount = 3750.00m, DateIssued = DateTime.Parse("2025-09-20"), Status = InvoiceStatus.Paid },
            new Invoice { Id = 10, InvoiceNumber = "INV-2025-010", ClientId = 2, Amount = 9800.00m, DateIssued = DateTime.Parse("2025-09-05"), Status = InvoiceStatus.Pending },
            new Invoice { Id = 11, InvoiceNumber = "INV-2025-011", ClientId = 9, Amount = 2450.00m, DateIssued = DateTime.Parse("2025-08-28"), Status = InvoiceStatus.Paid },
            new Invoice { Id = 12, InvoiceNumber = "INV-2025-012", ClientId = 10, Amount = 16700.00m, DateIssued = DateTime.Parse("2025-08-10"), Status = InvoiceStatus.Paid },
            new Invoice { Id = 13, InvoiceNumber = "INV-2025-013", ClientId = 11, Amount = 25000.00m, DateIssued = DateTime.Parse("2025-07-22"), Status = InvoiceStatus.Paid },
            new Invoice { Id = 14, InvoiceNumber = "INV-2025-014", ClientId = 12, Amount = 6200.00m, DateIssued = DateTime.Parse("2025-07-08"), Status = InvoiceStatus.Pending },
            new Invoice { Id = 15, InvoiceNumber = "INV-2025-015", ClientId = 13, Amount = 8400.00m, DateIssued = DateTime.Parse("2025-06-25"), Status = InvoiceStatus.Paid },
            new Invoice { Id = 16, InvoiceNumber = "INV-2025-016", ClientId = 14, Amount = 11900.00m, DateIssued = DateTime.Parse("2025-06-12"), Status = InvoiceStatus.Paid },
            new Invoice { Id = 17, InvoiceNumber = "INV-2025-017", ClientId = 15, Amount = 1450.00m, DateIssued = DateTime.Parse("2025-05-30"), Status = InvoiceStatus.Overdue },
            new Invoice { Id = 18, InvoiceNumber = "INV-2025-018", ClientId = 1, Amount = 4100.00m, DateIssued = DateTime.Parse("2025-05-15"), Status = InvoiceStatus.Paid },
            new Invoice { Id = 19, InvoiceNumber = "INV-2025-019", ClientId = 3, Amount = 3300.00m, DateIssued = DateTime.Parse("2025-04-20"), Status = InvoiceStatus.Paid },
            new Invoice { Id = 20, InvoiceNumber = "INV-2025-020", ClientId = 5, Amount = 7600.00m, DateIssued = DateTime.Parse("2025-04-05"), Status = InvoiceStatus.Paid },
            new Invoice { Id = 21, InvoiceNumber = "INV-2025-021", ClientId = 6, Amount = 9200.00m, DateIssued = DateTime.Parse("2025-03-18"), Status = InvoiceStatus.Paid },
            new Invoice { Id = 22, InvoiceNumber = "INV-2025-022", ClientId = 8, Amount = 5500.00m, DateIssued = DateTime.Parse("2025-03-02"), Status = InvoiceStatus.Pending },
            new Invoice { Id = 23, InvoiceNumber = "INV-2025-023", ClientId = 10, Amount = 12300.00m, DateIssued = DateTime.Parse("2025-02-20"), Status = InvoiceStatus.Paid },
            new Invoice { Id = 24, InvoiceNumber = "INV-2025-024", ClientId = 11, Amount = 8800.00m, DateIssued = DateTime.Parse("2025-02-05"), Status = InvoiceStatus.Paid }
        );

        modelBuilder.Entity<UserProfile>().HasData(
            new UserProfile
            {
                Id = 1,
                FullName = "Alex Morgan",
                Title = "Senior Product Manager",
                Email = "alex@billcorp.com",
                Phone = "+1 (555) 000-0000",
                Timezone = "Eastern Time (US & Canada) (GMT-05:00)",
                AvatarUrl = "",
                CompanyName = "TechFlow Solutions LLC",
                TaxId = "US-987654321",
                StreetAddress = "450 Enterprise Blvd, Suite 200",
                City = "San Francisco",
                ZipCode = "94105",
                Country = "United States",
                BrandColor = "#0F172A",
                SecondaryColor = "#F59E0B"
            }
        );
    }
}
