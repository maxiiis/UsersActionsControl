using Microsoft.EntityFrameworkCore.Migrations;

namespace EFModels.Migrations.LogDBMigrations
{
    public partial class AddUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventLogDatas_EventLogs_EventLogEventId",
                table: "EventLogDatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventLogDatas",
                table: "EventLogDatas");

            migrationBuilder.DropColumn(
                name: "Resourse",
                table: "EventLogs");

            migrationBuilder.DropColumn(
                name: "EventLogEventId",
                table: "EventLogDatas");

            migrationBuilder.DropColumn(
                name: "ResourseDepartment",
                table: "EventLogDatas");

            migrationBuilder.DropColumn(
                name: "ResourseFIO",
                table: "EventLogDatas");

            migrationBuilder.DropColumn(
                name: "ResourseFilial",
                table: "EventLogDatas");

            migrationBuilder.AddColumn<string>(
                name: "EventLogDataActivity",
                table: "EventLogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResourseId",
                table: "EventLogs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Activity",
                table: "EventLogDatas",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StatusText",
                table: "EventLogDatas",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventLogDatas",
                table: "EventLogDatas",
                column: "Activity");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Filial = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    Department = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventLogs_EventLogDataActivity",
                table: "EventLogs",
                column: "EventLogDataActivity");

            migrationBuilder.CreateIndex(
                name: "IX_EventLogs_ResourseId",
                table: "EventLogs",
                column: "ResourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventLogs_EventLogDatas_EventLogDataActivity",
                table: "EventLogs",
                column: "EventLogDataActivity",
                principalTable: "EventLogDatas",
                principalColumn: "Activity",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EventLogs_Users_ResourseId",
                table: "EventLogs",
                column: "ResourseId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventLogs_EventLogDatas_EventLogDataActivity",
                table: "EventLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_EventLogs_Users_ResourseId",
                table: "EventLogs");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_EventLogs_EventLogDataActivity",
                table: "EventLogs");

            migrationBuilder.DropIndex(
                name: "IX_EventLogs_ResourseId",
                table: "EventLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventLogDatas",
                table: "EventLogDatas");

            migrationBuilder.DropColumn(
                name: "EventLogDataActivity",
                table: "EventLogs");

            migrationBuilder.DropColumn(
                name: "ResourseId",
                table: "EventLogs");

            migrationBuilder.DropColumn(
                name: "Activity",
                table: "EventLogDatas");

            migrationBuilder.DropColumn(
                name: "StatusText",
                table: "EventLogDatas");

            migrationBuilder.AddColumn<string>(
                name: "Resourse",
                table: "EventLogs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EventLogEventId",
                table: "EventLogDatas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "ResourseDepartment",
                table: "EventLogDatas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResourseFIO",
                table: "EventLogDatas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResourseFilial",
                table: "EventLogDatas",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventLogDatas",
                table: "EventLogDatas",
                column: "EventLogEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventLogDatas_EventLogs_EventLogEventId",
                table: "EventLogDatas",
                column: "EventLogEventId",
                principalTable: "EventLogs",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
