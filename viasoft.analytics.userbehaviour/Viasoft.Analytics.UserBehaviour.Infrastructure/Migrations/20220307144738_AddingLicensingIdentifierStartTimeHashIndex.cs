using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Analytics.UserBehaviour.Infrastructure.Migrations
{
    public partial class AddingLicensingIdentifierStartTimeHashIndex : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingLicensingIdentifierStartTimeHashIndex(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LicenseUsageHistory_LicensingIdentifier_StartTime_Hash",
                table: "LicenseUsageHistory",
                columns: new[] { "LicensingIdentifier", "StartTime", "Hash" },
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LicenseUsageHistory_LicensingIdentifier_StartTime_Hash",
                table: "LicenseUsageHistory",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
