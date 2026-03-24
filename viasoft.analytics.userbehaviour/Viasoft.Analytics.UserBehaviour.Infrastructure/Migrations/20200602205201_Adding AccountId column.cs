using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Analytics.UserBehaviour.Infrastructure.Migrations
{
    public partial class AddingAccountIdcolumn : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public AddingAccountIdcolumn(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "LicenseUsageHistory",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "LicenseUsageHistory",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
