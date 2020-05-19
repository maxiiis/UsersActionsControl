using Microsoft.EntityFrameworkCore.Migrations;

namespace EFModels.Migrations.LogDBMigrations
{
    public partial class ActivityNameFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activivty",
                table: "EventLogs");

            migrationBuilder.DropColumn(
                name: "ActivivtyText",
                table: "EventLogDatas");

            migrationBuilder.DropColumn(
                name: "ActivivtyText2",
                table: "EventLogDatas");

            migrationBuilder.AddColumn<string>(
                name: "Activity",
                table: "EventLogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActivityText",
                table: "EventLogDatas",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActivityText2",
                table: "EventLogDatas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activity",
                table: "EventLogs");

            migrationBuilder.DropColumn(
                name: "ActivityText",
                table: "EventLogDatas");

            migrationBuilder.DropColumn(
                name: "ActivityText2",
                table: "EventLogDatas");

            migrationBuilder.AddColumn<string>(
                name: "Activivty",
                table: "EventLogs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActivivtyText",
                table: "EventLogDatas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ActivivtyText2",
                table: "EventLogDatas",
                type: "text",
                nullable: true);
        }
    }
}
