using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Analytics.UserBehaviour.Infrastructure.Migrations
{
    public partial class Set_TenantId_Unique_In_LicenseUsageHistoryIndex_Entity : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public Set_TenantId_Unique_In_LicenseUsageHistoryIndex_Entity(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LicenseUsageHistoryIndex_TenantId",
                table: "LicenseUsageHistoryIndex",
                column: "TenantId",
                unique: true,
                schema:_schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LicenseUsageHistoryIndex_TenantId",
                table: "LicenseUsageHistoryIndex",
                schema:_schemaNameProvider.GetSchemaName());
        }
    }
}
