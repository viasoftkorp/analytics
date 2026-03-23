using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Analytics.UserBehaviour.Infrastructure.Migrations
{
    public partial class AddingLicensingIdentifierEndTimeIndex : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingLicensingIdentifierEndTimeIndex(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LicenseUsageHistory_LicensingIdentifier_EndTime",
                table: "LicenseUsageHistory",
                columns: new[] { "LicensingIdentifier", "EndTime" },
                schema:_schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LicenseUsageHistory_LicensingIdentifier_EndTime",
                table: "LicenseUsageHistory",
                schema:_schemaNameProvider.GetSchemaName());
        }
    }
}
