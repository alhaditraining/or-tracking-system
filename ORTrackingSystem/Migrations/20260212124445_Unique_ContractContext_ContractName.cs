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
            migrationBuilder.Sql("""
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_ContractContexts_ContractName' AND object_id = OBJECT_ID(N'[ContractContexts]'))
    CREATE UNIQUE INDEX [IX_ContractContexts_ContractName] ON [ContractContexts]([ContractName]);

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_ContractContexts_CreatedByUserId' AND object_id = OBJECT_ID(N'[ContractContexts]'))
    CREATE INDEX [IX_ContractContexts_CreatedByUserId] ON [ContractContexts]([CreatedByUserId]);

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_ContractContexts_AspNetUsers_CreatedByUserId')
    ALTER TABLE [ContractContexts] ADD CONSTRAINT [FK_ContractContexts_AspNetUsers_CreatedByUserId]
        FOREIGN KEY ([CreatedByUserId]) REFERENCES [AspNetUsers]([Id]) ON DELETE NO ACTION;
""");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_ContractContexts_AspNetUsers_CreatedByUserId')
    ALTER TABLE [ContractContexts] DROP CONSTRAINT [FK_ContractContexts_AspNetUsers_CreatedByUserId];

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_ContractContexts_CreatedByUserId' AND object_id = OBJECT_ID(N'[ContractContexts]'))
    DROP INDEX [IX_ContractContexts_CreatedByUserId] ON [ContractContexts];

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'IX_ContractContexts_ContractName' AND object_id = OBJECT_ID(N'[ContractContexts]'))
    DROP INDEX [IX_ContractContexts_ContractName] ON [ContractContexts];
""");
        }
    }
}
