using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Analytics.UserBehaviour.Infrastructure.Migrations
{
    public partial class AddingFailureStackTracecolumn : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingFailureStackTracecolumn(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FailureStackTrace",
                table: "LicenseUsageHistoryIndex",
                type: "nvarchar(max)",
                nullable: true,
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailureStackTrace",
                table: "LicenseUsageHistoryIndex",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
