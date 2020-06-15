using Microsoft.EntityFrameworkCore.Migrations;

namespace EFModels.Migrations.MainDBMigration
{
    public partial class AlertsFKFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_BPCases_BPCaseCaseId",
                table: "Alerts");

            migrationBuilder.DropIndex(
                name: "IX_Alerts_BPCaseCaseId",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "BPCaseCaseId",
                table: "Alerts");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_CaseId",
                table: "Alerts",
                column: "CaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_BPCases_CaseId",
                table: "Alerts",
                column: "CaseId",
                principalTable: "BPCases",
                principalColumn: "CaseId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alerts_BPCases_CaseId",
                table: "Alerts");

            migrationBuilder.DropIndex(
                name: "IX_Alerts_CaseId",
                table: "Alerts");

            migrationBuilder.AddColumn<long>(
                name: "BPCaseCaseId",
                table: "Alerts",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_BPCaseCaseId",
                table: "Alerts",
                column: "BPCaseCaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alerts_BPCases_BPCaseCaseId",
                table: "Alerts",
                column: "BPCaseCaseId",
                principalTable: "BPCases",
                principalColumn: "CaseId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
