using Microsoft.EntityFrameworkCore.Migrations;

namespace EFModels.Migrations.LogDBMigrations
{
    public partial class RemoveIdFromLogData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventLogDatas_EventLogs_eventLogEventId",
                table: "EventLogDatas");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "EventLogDatas");

            migrationBuilder.RenameColumn(
                name: "eventLogEventId",
                table: "EventLogDatas",
                newName: "EventLogEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventLogDatas_EventLogs_EventLogEventId",
                table: "EventLogDatas",
                column: "EventLogEventId",
                principalTable: "EventLogs",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventLogDatas_EventLogs_EventLogEventId",
                table: "EventLogDatas");

            migrationBuilder.RenameColumn(
                name: "EventLogEventId",
                table: "EventLogDatas",
                newName: "eventLogEventId");

            migrationBuilder.AddColumn<long>(
                name: "EventId",
                table: "EventLogDatas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_EventLogDatas_EventLogs_eventLogEventId",
                table: "EventLogDatas",
                column: "eventLogEventId",
                principalTable: "EventLogs",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
