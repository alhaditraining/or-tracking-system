using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ORTrackingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddDisplayNameToAspNetUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
IF COL_LENGTH('AspNetUsers', 'DisplayName') IS NULL
    ALTER TABLE [AspNetUsers] ADD [DisplayName] nvarchar(max) NULL;
""");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
IF COL_LENGTH('AspNetUsers', 'DisplayName') IS NOT NULL
    ALTER TABLE [AspNetUsers] DROP COLUMN [DisplayName];
""");
        }
    }
}
