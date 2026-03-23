using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Analytics.UserBehaviour.Infrastructure.Migrations
{
    public partial class AddingLicensingModelandLicensignModetoLicenseUsageHistory : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingLicensingModelandLicensignModetoLicenseUsageHistory(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LicensingMode",
                table: "LicenseUsageHistory",
                nullable: true,
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.AddColumn<int>(
                name: "LicensingModel",
                table: "LicenseUsageHistory",
                nullable: false,
                defaultValue: 0,
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicensingMode",
                table: "LicenseUsageHistory",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.DropColumn(
                name: "LicensingModel",
                table: "LicenseUsageHistory",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
