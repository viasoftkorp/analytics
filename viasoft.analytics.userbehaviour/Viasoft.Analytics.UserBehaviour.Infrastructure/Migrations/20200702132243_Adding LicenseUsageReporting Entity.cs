using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Analytics.UserBehaviour.Infrastructure.Migrations
{
    public partial class AddingLicenseUsageReportingEntity : Migration
    {
        
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingLicenseUsageReportingEntity(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LicenseUsageReporting",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LicensingIdentifier = table.Column<Guid>(nullable: false),
                    StartInterval = table.Column<DateTime>(nullable: false),
                    EndInterval = table.Column<DateTime>(nullable: false),
                    UsageCount = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseUsageReporting", x => x.Id);
                },
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LicenseUsageReporting",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
