using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Viasoft.Core.Storage.Schema;

namespace Viasoft.Analytics.UserBehaviour.Infrastructure.PostgreSql.Migrations
{
    public partial class Added_Entities : Migration
    {
        private readonly ISchemaNameProvider _schemaNameProvider;

        public Added_Entities(ISchemaNameProvider schemaNameProvider)
        {
            _schemaNameProvider = schemaNameProvider;
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "licenseusagehistory",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    licensingidentifier = table.Column<Guid>(type: "uuid", nullable: false),
                    appidentifier = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    appname = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    bundleidentifier = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    bundlename = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    softwarename = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    softwareidentifier = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    applicenses = table.Column<int>(type: "integer", nullable: false),
                    applicensesconsumed = table.Column<int>(type: "integer", nullable: false),
                    additionallicenses = table.Column<int>(type: "integer", nullable: false),
                    additionallicensesconsumed = table.Column<int>(type: "integer", nullable: false),
                    additionallicense = table.Column<bool>(type: "boolean", nullable: false),
                    starttime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    endtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    user = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    cnpj = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    lastupdate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    licensingstatus = table.Column<int>(type: "integer", nullable: false),
                    appstatus = table.Column<int>(type: "integer", nullable: false),
                    softwareversion = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    hostname = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    hostuser = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    localipaddress = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    language = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    osinfo = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    browserinfo = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    databasename = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    hash = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    accountname = table.Column<string>(type: "character varying(450)", maxLength: 450, nullable: true),
                    accountid = table.Column<Guid>(type: "uuid", nullable: false),
                    domain = table.Column<int>(type: "integer", nullable: false),
                    licensingmodel = table.Column<int>(type: "integer", nullable: false),
                    licensingmode = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_licenseusagehistory", x => x.id);
                },
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "licenseusagehistoryindex",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false),
                    isindexing = table.Column<bool>(type: "boolean", nullable: false),
                    hasesfailedsincelastreindex = table.Column<bool>(type: "boolean", nullable: false),
                    lastmodificationtime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    reindexidentifier = table.Column<Guid>(type: "uuid", nullable: true),
                    failurestacktrace = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_licenseusagehistoryindex", x => x.id);
                },
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateTable(
                name: "licenseusagereporting",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    licensingidentifier = table.Column<Guid>(type: "uuid", nullable: false),
                    startinterval = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    endinterval = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    usagecount = table.Column<int>(type: "integer", nullable: false),
                    day = table.Column<int>(type: "integer", nullable: false),
                    tenantid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_licenseusagereporting", x => x.id);
                },
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateIndex(
                name: "ix_licenseusagehistory_licensingidentifier_endtime",
                table: "licenseusagehistory",
                columns: new[] { "licensingidentifier", "endtime" },
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateIndex(
                name: "ix_licenseusagehistory_licensingidentifier_hash_endtime",
                table: "licenseusagehistory",
                columns: new[] { "licensingidentifier", "hash", "endtime" },
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.CreateIndex(
                name: "ix_licenseusagehistoryindex_tenantid",
                table: "licenseusagehistoryindex",
                column: "tenantid",
                unique: true,
                schema: _schemaNameProvider.GetSchemaName());
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "licenseusagehistory",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.DropTable(
                name: "licenseusagehistoryindex",
                schema: _schemaNameProvider.GetSchemaName());

            migrationBuilder.DropTable(
                name: "licenseusagereporting",
                schema: _schemaNameProvider.GetSchemaName());
        }
    }
}
