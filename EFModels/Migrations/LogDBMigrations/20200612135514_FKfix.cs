using Microsoft.EntityFrameworkCore.Migrations;

namespace EFModels.Migrations.LogDBMigrations
{
    public partial class FKfix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventLogs_EventLogDatas_EventLogDataActivity",
                table: "EventLogs");

            migrationBuilder.DropTable(
                name: "EventLogDatas");

            migrationBuilder.DropIndex(
                name: "IX_EventLogs_EventLogDataActivity",
                table: "EventLogs");

            migrationBuilder.DropColumn(
                name: "Activity",
                table: "EventLogs");

            migrationBuilder.DropColumn(
                name: "EventLogDataActivity",
                table: "EventLogs");

            migrationBuilder.AddColumn<string>(
                name: "ActivityId",
                table: "EventLogs",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ActivityText = table.Column<string>(nullable: true),
                    ActivityText2 = table.Column<string>(nullable: true),
                    StatusText = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventLogs_ActivityId",
                table: "EventLogs",
                column: "ActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventLogs_Activities_ActivityId",
                table: "EventLogs",
                column: "ActivityId",
                principalTable: "Activities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventLogs_Activities_ActivityId",
                table: "EventLogs");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_EventLogs_ActivityId",
                table: "EventLogs");

            migrationBuilder.DropColumn(
                name: "ActivityId",
                table: "EventLogs");

            migrationBuilder.AddColumn<string>(
                name: "Activity",
                table: "EventLogs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EventLogDataActivity",
                table: "EventLogs",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EventLogDatas",
                columns: table => new
                {
                    Activity = table.Column<string>(type: "text", nullable: false),
                    ActivityText = table.Column<string>(type: "text", nullable: true),
                    ActivityText2 = table.Column<string>(type: "text", nullable: true),
                    StatusText = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventLogDatas", x => x.Activity);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventLogs_EventLogDataActivity",
                table: "EventLogs",
                column: "EventLogDataActivity");

            migrationBuilder.AddForeignKey(
                name: "FK_EventLogs_EventLogDatas_EventLogDataActivity",
                table: "EventLogs",
                column: "EventLogDataActivity",
                principalTable: "EventLogDatas",
                principalColumn: "Activity",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
