using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Analytics.UserBehaviour.Infrastructure.Migrations
{
    public partial class Add_LicenseUsageHistoryIndex_Entity : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public Add_LicenseUsageHistoryIndex_Entity(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LicenseUsageHistoryIndex",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<Guid>(nullable: false),
                    IsIndexing = table.Column<bool>(nullable: false),
                    HasEsFailedSinceLastReindex = table.Column<bool>(nullable: false),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    ReindexTries = table.Column<int>(nullable: false),
                    ReindexIdentifier = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseUsageHistoryIndex", x => x.Id);
                },
                schema:_schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LicenseUsageHistoryIndex",
                schema:_schemaNameProvider.GetSchemaName());
        }
    }
}
