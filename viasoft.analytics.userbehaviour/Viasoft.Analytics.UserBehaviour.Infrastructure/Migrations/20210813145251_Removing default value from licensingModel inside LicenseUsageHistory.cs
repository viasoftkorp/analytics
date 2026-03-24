using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Analytics.UserBehaviour.Infrastructure.Migrations
{
    public partial class RemovingdefaultvaluefromlicensingModelinsideLicenseUsageHistory : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public RemovingdefaultvaluefromlicensingModelinsideLicenseUsageHistory(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LicensingModel",
                table: "LicenseUsageHistory",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0,
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LicensingModel",
                table: "LicenseUsageHistory",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
