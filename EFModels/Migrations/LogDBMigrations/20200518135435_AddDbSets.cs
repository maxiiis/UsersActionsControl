using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace EFModels.Migrations.LogDBMigrations
{
    public partial class AddDbSets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventLogs",
                columns: table => new
                {
                    EventId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CaseId = table.Column<long>(nullable: false),
                    Resourse = table.Column<string>(nullable: true),
                    Activivty = table.Column<string>(nullable: true),
                    AnalysStatus = table.Column<bool>(nullable: false),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Cost = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventLogs", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "EventLogDatas",
                columns: table => new
                {
                    eventLogEventId = table.Column<long>(nullable: false),
                    EventId = table.Column<long>(nullable: false),
                    ResourseFIO = table.Column<string>(nullable: true),
                    ResourseDepartment = table.Column<string>(nullable: true),
                    ResourseFilial = table.Column<string>(nullable: true),
                    ActivivtyText = table.Column<string>(nullable: true),
                    ActivivtyText2 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventLogDatas", x => x.eventLogEventId);
                    table.ForeignKey(
                        name: "FK_EventLogDatas_EventLogs_eventLogEventId",
                        column: x => x.eventLogEventId,
                        principalTable: "EventLogs",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventLogDatas");

            migrationBuilder.DropTable(
                name: "EventLogs");
        }
    }
}
