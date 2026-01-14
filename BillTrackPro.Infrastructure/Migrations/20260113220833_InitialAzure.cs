using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BillTrackPro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialAzure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timezone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaxId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyLogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrandColor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecondaryColor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateIssued = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "AvatarUrl", "CompanyName", "Email", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "https://i.pravatar.cc/150?u=1", "Apex Logistics", "contact@apexlogistics.com", true, "Apex Logistics LLC" },
                    { 2, "https://i.pravatar.cc/150?u=2", "Globex Corporation", "info@globex.com", true, "Globex Inc" },
                    { 3, "https://i.pravatar.cc/150?u=3", "Soylent Corp", "sales@soylent.com", false, "Soylent Corp" },
                    { 4, "https://i.pravatar.cc/150?u=4", "Initech Systems", "bill@initech.com", true, "Initech" },
                    { 5, "https://i.pravatar.cc/150?u=5", "Umbrella Corp", "alice@umbrella.com", true, "Umbrella Corp" },
                    { 6, "https://i.pravatar.cc/150?u=6", "Stark Industries", "tony@stark.com", true, "Stark Industries" },
                    { 7, "https://i.pravatar.cc/150?u=7", "Cyberdyne", "skynet@cyberdyne.com", true, "Cyberdyne Systems" },
                    { 8, "https://i.pravatar.cc/150?u=8", "Massive Dynamic", "nina@massive.com", true, "Massive Dynamic" },
                    { 9, "https://i.pravatar.cc/150?u=9", "Hooli XYZ", "gavin@hooli.com", false, "Hooli" },
                    { 10, "https://i.pravatar.cc/150?u=10", "Pied Piper", "richard@piedpiper.com", true, "Pied Piper" },
                    { 11, "https://i.pravatar.cc/150?u=11", "Wayne Enterprises", "bruce@wayne.com", true, "Wayne Enterprises" },
                    { 12, "https://i.pravatar.cc/150?u=12", "Acme Corp", "roadrunner@acme.com", true, "Acme Corp" },
                    { 13, "https://i.pravatar.cc/150?u=13", "Oscorp Industries", "norman@oscorp.com", true, "Oscorp" },
                    { 14, "https://i.pravatar.cc/150?u=14", "Nakatomi", "takagi@nakatomi.com", true, "Nakatomi Trading" },
                    { 15, "https://i.pravatar.cc/150?u=15", "Tyrell Corporation", "elden@tyrell.com", false, "Tyrell Corp" }
                });

            migrationBuilder.InsertData(
                table: "UserProfiles",
                columns: new[] { "Id", "AvatarUrl", "BrandColor", "City", "CompanyLogoUrl", "CompanyName", "Country", "Email", "FullName", "Phone", "SecondaryColor", "StreetAddress", "TaxId", "Timezone", "Title", "ZipCode" },
                values: new object[] { 1, "", "#0F172A", "San Francisco", "", "TechFlow Solutions LLC", "United States", "alex@billcorp.com", "Alex Morgan", "+1 (555) 000-0000", "#F59E0B", "450 Enterprise Blvd, Suite 200", "US-987654321", "Eastern Time (US & Canada) (GMT-05:00)", "Senior Product Manager", "94105" });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "Id", "Amount", "ClientId", "DateIssued", "InvoiceNumber", "Status" },
                values: new object[,]
                {
                    { 1, 12450.32m, 1, new DateTime(2026, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2026-001", 1 },
                    { 2, 3450.00m, 2, new DateTime(2026, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2026-002", 0 },
                    { 3, 8900.00m, 3, new DateTime(2025, 12, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2025-003", 1 },
                    { 4, 2100.00m, 4, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2025-004", 1 },
                    { 5, 5600.00m, 5, new DateTime(2025, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2025-005", 2 },
                    { 6, 15420.00m, 6, new DateTime(2025, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2025-006", 1 },
                    { 7, 4500.00m, 7, new DateTime(2025, 10, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2025-007", 1 },
                    { 8, 7200.00m, 8, new DateTime(2025, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2025-008", 1 },
                    { 9, 3750.00m, 1, new DateTime(2025, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2025-009", 1 },
                    { 10, 9800.00m, 2, new DateTime(2025, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2025-010", 0 },
                    { 11, 2450.00m, 9, new DateTime(2025, 8, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2025-011", 1 },
                    { 12, 16700.00m, 10, new DateTime(2025, 8, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2025-012", 1 },
                    { 13, 25000.00m, 11, new DateTime(2025, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2025-013", 1 },
                    { 14, 6200.00m, 12, new DateTime(2025, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2025-014", 0 },
                    { 15, 8400.00m, 13, new DateTime(2025, 6, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2025-015", 1 },
                    { 16, 11900.00m, 14, new DateTime(2025, 6, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2025-016", 1 },
                    { 17, 1450.00m, 15, new DateTime(2025, 5, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2025-017", 2 },
                    { 18, 4100.00m, 1, new DateTime(2025, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2025-018", 1 },
                    { 19, 3300.00m, 3, new DateTime(2025, 4, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2025-019", 1 },
                    { 20, 7600.00m, 5, new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2025-020", 1 },
                    { 21, 9200.00m, 6, new DateTime(2025, 3, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2025-021", 1 },
                    { 22, 5500.00m, 8, new DateTime(2025, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2025-022", 0 },
                    { 23, 12300.00m, 10, new DateTime(2025, 2, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2025-023", 1 },
                    { 24, 8800.00m, 11, new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2025-024", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ClientId",
                table: "Invoices",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
