using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Analytics.UserBehaviour.Infrastructure.Migrations
{
    public partial class AddingLicensingIdentifierhashandendTimeindex : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingLicensingIdentifierhashandendTimeindex(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LicenseUsageHistory_LicensingIdentifier_Hash_EndTime",
                table: "LicenseUsageHistory",
                columns: new[] { "LicensingIdentifier", "Hash", "EndTime" },
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LicenseUsageHistory_LicensingIdentifier_Hash_EndTime",
                table: "LicenseUsageHistory",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
