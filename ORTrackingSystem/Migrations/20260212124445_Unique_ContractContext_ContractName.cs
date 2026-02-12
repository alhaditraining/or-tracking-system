using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ORTrackingSystem.Migrations
{
    /// <inheritdoc />
    public partial class Unique_ContractContext_ContractName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractContexts_ContractName",
                table: "ContractContexts",
                column: "ContractName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContractContexts_CreatedByUserId",
                table: "ContractContexts",
                column: "CreatedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContractContexts_AspNetUsers_CreatedByUserId",
                table: "ContractContexts",
                column: "CreatedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContractContexts_AspNetUsers_CreatedByUserId",
                table: "ContractContexts");

            migrationBuilder.DropIndex(
                name: "IX_ContractContexts_ContractName",
                table: "ContractContexts");

            migrationBuilder.DropIndex(
                name: "IX_ContractContexts_CreatedByUserId",
                table: "ContractContexts");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "AspNetUsers");
        }
    }
}
