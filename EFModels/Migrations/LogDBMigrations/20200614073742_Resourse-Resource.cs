using Microsoft.EntityFrameworkCore.Migrations;

namespace EFModels.Migrations.LogDBMigrations
{
    public partial class ResourseResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventLogs_Users_ResourseId",
                table: "EventLogs");

            migrationBuilder.DropIndex(
                name: "IX_EventLogs_ResourseId",
                table: "EventLogs");

            migrationBuilder.DropColumn(
                name: "ResourseId",
                table: "EventLogs");

            migrationBuilder.AddColumn<string>(
                name: "ResourceId",
                table: "EventLogs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventLogs_ResourceId",
                table: "EventLogs",
                column: "ResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventLogs_Users_ResourceId",
                table: "EventLogs",
                column: "ResourceId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventLogs_Users_ResourceId",
                table: "EventLogs");

            migrationBuilder.DropIndex(
                name: "IX_EventLogs_ResourceId",
                table: "EventLogs");

            migrationBuilder.DropColumn(
                name: "ResourceId",
                table: "EventLogs");

            migrationBuilder.AddColumn<string>(
                name: "ResourseId",
                table: "EventLogs",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventLogs_ResourseId",
                table: "EventLogs",
                column: "ResourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventLogs_Users_ResourseId",
                table: "EventLogs",
                column: "ResourseId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
