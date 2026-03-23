using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Analytics.UserBehaviour.Infrastructure.Migrations
{
    public partial class RemovedtheLicenseUsageEntity : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public RemovedtheLicenseUsageEntity(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LicenseUsage",
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LicenseUsage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdditionalLicense = table.Column<bool>(type: "bit", nullable: false),
                    AppIdentifier = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    AppName = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    BundleIdentifier = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    BundleName = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Cnpj = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    DurationInMinutes = table.Column<int>(type: "int", nullable: false),
                    DurationInSeconds = table.Column<int>(type: "int", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LicenseIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LogDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoftwareIdentifier = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    SoftwareName = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    User = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseUsage", x => x.Id);
                },
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
