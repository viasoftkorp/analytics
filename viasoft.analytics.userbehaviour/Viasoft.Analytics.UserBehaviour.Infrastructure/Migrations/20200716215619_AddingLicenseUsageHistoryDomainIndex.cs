using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Analytics.UserBehaviour.Infrastructure.Migrations
{
    public partial class AddingLicenseUsageHistoryDomainIndex : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingLicenseUsageHistoryDomainIndex(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LicenseUsageHistory_Domain",
                table: "LicenseUsageHistory",
                column: "Domain", schema:_schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LicenseUsageHistory_Domain",
                table: "LicenseUsageHistory", schema:_schemaNameProvider.GetSchemaName());
        }
    }
}
