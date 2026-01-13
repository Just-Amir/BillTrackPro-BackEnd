using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BillTrackPro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedDataUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                column: "AvatarUrl",
                value: "https://i.pravatar.cc/150?u=1");

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2,
                column: "AvatarUrl",
                value: "https://i.pravatar.cc/150?u=2");

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 3,
                column: "AvatarUrl",
                value: "https://i.pravatar.cc/150?u=3");

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 4,
                column: "AvatarUrl",
                value: "https://i.pravatar.cc/150?u=4");

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "AvatarUrl", "CompanyName", "Email", "IsActive", "Name" },
                values: new object[,]
                {
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
                table: "Invoices",
                columns: new[] { "Id", "Amount", "ClientId", "DateIssued", "InvoiceNumber", "Status" },
                values: new object[,]
                {
                    { 9, 750.00m, 1, new DateTime(2023, 10, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2024-009", "Paid" },
                    { 10, 9800.00m, 2, new DateTime(2023, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2024-010", "Pending" },
                    { 18, 2100.00m, 1, new DateTime(2023, 10, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2024-018", "Paid" },
                    { 19, 300.00m, 3, new DateTime(2023, 10, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2024-019", "Overdue" },
                    { 5, 5600.00m, 5, new DateTime(2023, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2024-005", "Pending" },
                    { 6, 10420.00m, 6, new DateTime(2023, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2024-006", "Paid" },
                    { 7, 1500.00m, 7, new DateTime(2023, 10, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2024-007", "Overdue" },
                    { 8, 3200.00m, 8, new DateTime(2023, 10, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2024-008", "Paid" },
                    { 11, 450.00m, 9, new DateTime(2023, 10, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2024-011", "Overdue" },
                    { 12, 16700.00m, 10, new DateTime(2023, 10, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2024-012", "Paid" },
                    { 13, 25000.00m, 11, new DateTime(2023, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2024-013", "Paid" },
                    { 14, 1200.00m, 12, new DateTime(2023, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2024-014", "Pending" },
                    { 15, 3400.00m, 13, new DateTime(2023, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2024-015", "Paid" },
                    { 16, 8900.00m, 14, new DateTime(2023, 10, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2024-016", "Pending" },
                    { 17, 450.00m, 15, new DateTime(2023, 10, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2024-017", "Overdue" },
                    { 20, 4600.00m, 5, new DateTime(2023, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "INV-2024-020", "Pending" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Invoices",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                column: "AvatarUrl",
                value: "AL");

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2,
                column: "AvatarUrl",
                value: "GI");

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 3,
                column: "AvatarUrl",
                value: "SC");

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 4,
                column: "AvatarUrl",
                value: "IT");
        }
    }
}
