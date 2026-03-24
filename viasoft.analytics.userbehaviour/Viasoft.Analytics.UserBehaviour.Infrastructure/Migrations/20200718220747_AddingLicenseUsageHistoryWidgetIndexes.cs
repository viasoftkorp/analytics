using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Analytics.UserBehaviour.Infrastructure.Migrations
{
    public partial class AddingLicenseUsageHistoryWidgetIndexes : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingLicenseUsageHistoryWidgetIndexes(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LicenseUsageHistory_AccountName_Properties",
                table: "LicenseUsageHistory",
                column: "AccountName", schema:_schemaNameProvider.GetSchemaName())
                .Annotation("SqlServer:Include", new[] { "AppIdentifier", "AppName", "Domain", "LicensingIdentifier", "AccountId" });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseUsageHistory_AppName_Properties",
                table: "LicenseUsageHistory",
                column: "AppName", schema:_schemaNameProvider.GetSchemaName())
                .Annotation("SqlServer:Include", new[] { "AppIdentifier", "Domain", "LicensingIdentifier", "AccountName", "AccountId" });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseUsageHistory_Domain_Properties",
                table: "LicenseUsageHistory",
                column: "Domain", schema:_schemaNameProvider.GetSchemaName())
                .Annotation("SqlServer:Include", new[] { "AppIdentifier", "AppName", "LicensingIdentifier", "AccountName", "AccountId" });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseUsageHistory_SoftwareName_Properties",
                table: "LicenseUsageHistory",
                column: "SoftwareName", schema:_schemaNameProvider.GetSchemaName())
                .Annotation("SqlServer:Include", new[] { "AppIdentifier", "AppName", "Domain", "LicensingIdentifier", "AccountName", "AccountId" });

            migrationBuilder.CreateIndex(
                name: "IX_LicenseUsageHistory_AccountName_EndTime",
                table: "LicenseUsageHistory",
                columns: new[] { "AccountName", "EndTime" }, schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateIndex(
                name: "IX_LicenseUsageHistory_AppName_EndTime",
                table: "LicenseUsageHistory",
                columns: new[] { "AppName", "EndTime" }, schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateIndex(
                name: "IX_LicenseUsageHistory_Domain_EndTime",
                table: "LicenseUsageHistory",
                columns: new[] { "Domain", "EndTime" }, schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateIndex(
                name: "IX_LicenseUsageHistory_SoftwareName_EndTime",
                table: "LicenseUsageHistory",
                columns: new[] { "SoftwareName", "EndTime" }, schema:_schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LicenseUsageHistory_AccountName_Properties",
                table: "LicenseUsageHistory", schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.DropIndex(
                name: "IX_LicenseUsageHistory_AppName_Properties",
                table: "LicenseUsageHistory", schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.DropIndex(
                name: "IX_LicenseUsageHistory_Domain_Properties",
                table: "LicenseUsageHistory", schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.DropIndex(
                name: "IX_LicenseUsageHistory_SoftwareName_Properties",
                table: "LicenseUsageHistory", schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.DropIndex(
                name: "IX_LicenseUsageHistory_AccountName_EndTime",
                table: "LicenseUsageHistory", schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.DropIndex(
                name: "IX_LicenseUsageHistory_AppName_EndTime",
                table: "LicenseUsageHistory", schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.DropIndex(
                name: "IX_LicenseUsageHistory_Domain_EndTime",
                table: "LicenseUsageHistory", schema:_schemaNameProvider.GetSchemaName());

            migrationBuilder.DropIndex(
                name: "IX_LicenseUsageHistory_SoftwareName_EndTime",
                table: "LicenseUsageHistory", schema:_schemaNameProvider.GetSchemaName());
        }
    }
}
