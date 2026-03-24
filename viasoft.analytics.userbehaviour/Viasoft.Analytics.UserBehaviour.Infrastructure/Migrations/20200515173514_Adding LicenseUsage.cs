using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Analytics.UserBehaviour.Infrastructure.Migrations
{
    public partial class AddingLicenseUsage : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingLicenseUsage(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LicenseUsage",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LicenseIdentifier = table.Column<Guid>(nullable: false),
                    Cnpj = table.Column<string>(maxLength: 450, nullable: true),
                    User = table.Column<string>(maxLength: 450, nullable: true),
                    AppIdentifier = table.Column<string>(maxLength: 450, nullable: true),
                    AppName = table.Column<string>(maxLength: 450, nullable: true),
                    BundleIdentifier = table.Column<string>(maxLength: 450, nullable: true),
                    BundleName = table.Column<string>(maxLength: 450, nullable: true),
                    SoftwareIdentifier = table.Column<string>(maxLength: 450, nullable: true),
                    SoftwareName = table.Column<string>(maxLength: 450, nullable: true),
                    AdditionalLicense = table.Column<bool>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    DurationInSeconds = table.Column<int>(nullable: false),
                    DurationInMinutes = table.Column<int>(nullable: false),
                    LogDateTime = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseUsage", x => x.Id);
                },
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LicenseUsage", schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
