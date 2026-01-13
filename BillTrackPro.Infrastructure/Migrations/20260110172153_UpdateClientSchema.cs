using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BillTrackPro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClientSchema : Migration
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
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DateIssued = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    { 1, "AL", "Apex Logistics", "contact@apexlogistics.com", true, "Apex Logistics LLC" },
                    { 2, "GI", "Globex Corporation", "info@globex.com", true, "Globex Inc" },
                    { 3, "SC", "Soylent Corp", "sales@soylent.com", false, "Soylent Corp" },
                    { 4, "IT", "Initech Systems", "bill@initech.com", true, "Initech" }
                });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "Id", "Amount", "ClientId", "DateIssued", "InvoiceNumber", "Status" },
                values: new object[,]
                {
                    { 1, 12450.32m, 1, new DateTime(2023, 10, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2024-001", "Paid" },
                    { 2, 3450.00m, 2, new DateTime(2023, 10, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2024-002", "Pending" },
                    { 3, 890.00m, 3, new DateTime(2023, 10, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2024-003", "Overdue" },
                    { 4, 2100.00m, 4, new DateTime(2023, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2024-004", "Paid" }
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
                name: "Clients");
        }
    }
}
