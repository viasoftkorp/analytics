using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Analytics.UserBehaviour.Infrastructure.Migrations
{
    public partial class AddingAppNameAppIdentifierIndexandLicensingIdentifierAccountNameAccountIdIndex : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingAppNameAppIdentifierIndexandLicensingIdentifierAccountNameAccountIdIndex(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LicenseUsageHistory_AppName_AppIdentifier",
                table: "LicenseUsageHistory",
                columns: new[] { "AppName", "AppIdentifier" },
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateIndex(
                name: "IX_LicenseUsageHistory_LicensingIdentifier_AccountName_AccountId",
                table: "LicenseUsageHistory",
                columns: new[] { "LicensingIdentifier", "AccountName", "AccountId" },
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LicenseUsageHistory_AppName_AppIdentifier",
                table: "LicenseUsageHistory",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.DropIndex(
                name: "IX_LicenseUsageHistory_LicensingIdentifier_AccountName_AccountId",
                table: "LicenseUsageHistory",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
