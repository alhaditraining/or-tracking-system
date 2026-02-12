using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ORTrackingSystem.Migrations
{
    /// <inheritdoc />
    public partial class UniqueStationeryInvoiceNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_StationeryInvoices_InvoiceNumber",
                table: "StationeryInvoices",
                column: "InvoiceNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StationeryInvoices_InvoiceNumber",
                table: "StationeryInvoices");
        }
    }
}
