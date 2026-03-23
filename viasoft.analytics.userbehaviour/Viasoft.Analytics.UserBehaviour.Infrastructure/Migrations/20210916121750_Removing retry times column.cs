using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Analytics.UserBehaviour.Infrastructure.Migrations
{
    public partial class Removingretrytimescolumn : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public Removingretrytimescolumn(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReindexTries",
                table: "LicenseUsageHistoryIndex",
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReindexTries",
                table: "LicenseUsageHistoryIndex",
                type: "int",
                nullable: false,
                defaultValue: 0,
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
