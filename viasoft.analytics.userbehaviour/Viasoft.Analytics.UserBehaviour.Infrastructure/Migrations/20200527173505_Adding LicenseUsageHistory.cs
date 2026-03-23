using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Analytics.UserBehaviour.Infrastructure.Migrations
{
    public partial class AddingLicenseUsageHistory : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingLicenseUsageHistory(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LicenseUsageHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LicensingIdentifier = table.Column<Guid>(nullable: false),
                    AppIdentifier = table.Column<string>(maxLength: 450, nullable: true),
                    AppName = table.Column<string>(maxLength: 450, nullable: true),
                    BundleIdentifier = table.Column<string>(maxLength: 450, nullable: true),
                    BundleName = table.Column<string>(maxLength: 450, nullable: true),
                    SoftwareName = table.Column<string>(maxLength: 450, nullable: true),
                    SoftwareIdentifier = table.Column<string>(maxLength: 450, nullable: true),
                    AppLicenses = table.Column<int>(nullable: false),
                    AppLicensesConsumed = table.Column<int>(nullable: false),
                    AdditionalLicenses = table.Column<int>(nullable: false),
                    AdditionalLicensesConsumed = table.Column<int>(nullable: false),
                    AdditionalLicense = table.Column<bool>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: true),
                    User = table.Column<string>(maxLength: 450, nullable: true),
                    Cnpj = table.Column<string>(maxLength: 450, nullable: true),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    LicensingStatus = table.Column<int>(nullable: false),
                    AppStatus = table.Column<int>(nullable: false),
                    SoftwareVersion = table.Column<string>(maxLength: 450, nullable: true),
                    HostName = table.Column<string>(maxLength: 450, nullable: true),
                    HostUser = table.Column<string>(maxLength: 450, nullable: true),
                    LocalIpAddress = table.Column<string>(maxLength: 450, nullable: true),
                    Language = table.Column<string>(maxLength: 450, nullable: true),
                    OsInfo = table.Column<string>(maxLength: 450, nullable: true),
                    BrowserInfo = table.Column<string>(maxLength: 450, nullable: true),
                    DatabaseName = table.Column<string>(maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseUsageHistory", x => x.Id);
                },
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LicenseUsageHistory",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
