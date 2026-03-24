using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Analytics.UserBehaviour.Infrastructure.PostgreSql.Migrations
{
    public partial class AddingLicensingIdentifierStartTimeHashIndex : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;
        
        public AddingLicensingIdentifierStartTimeHashIndex(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_licenseusagehistory_licensingidentifier_starttime_hash",
                table: "licenseusagehistory",
                columns: new[] { "licensingidentifier", "starttime", "hash" },
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_licenseusagehistory_licensingidentifier_starttime_hash",
                table: "licenseusagehistory",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
