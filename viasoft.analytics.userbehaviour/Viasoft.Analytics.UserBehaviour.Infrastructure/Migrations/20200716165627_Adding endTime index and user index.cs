using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Analytics.UserBehaviour.Infrastructure.Migrations
{
    public partial class AddingendTimeindexanduserindex : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingendTimeindexanduserindex(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LicenseUsageHistory_EndTime",
                table: "LicenseUsageHistory",
                column: "EndTime",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateIndex(
                name: "IX_LicenseUsageHistory_User",
                table: "LicenseUsageHistory",
                column: "User",
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LicenseUsageHistory_EndTime",
                table: "LicenseUsageHistory",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.DropIndex(
                name: "IX_LicenseUsageHistory_User",
                table: "LicenseUsageHistory",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
