using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillTrackPro.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "BillTrack");

            migrationBuilder.RenameTable(
                name: "UserProfiles",
                newName: "UserProfiles",
                newSchema: "BillTrack");

            migrationBuilder.RenameTable(
                name: "Invoices",
                newName: "Invoices",
                newSchema: "BillTrack");

            migrationBuilder.RenameTable(
                name: "Clients",
                newName: "Clients",
                newSchema: "BillTrack");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "UserProfiles",
                schema: "BillTrack",
                newName: "UserProfiles");

            migrationBuilder.RenameTable(
                name: "Invoices",
                schema: "BillTrack",
                newName: "Invoices");

            migrationBuilder.RenameTable(
                name: "Clients",
                schema: "BillTrack",
                newName: "Clients");
        }
    }
}
