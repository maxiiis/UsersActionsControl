using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace EFModels.Migrations.MainDBMigration
{
    public partial class AddAlerts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alerts",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BPId = table.Column<long>(nullable: false),
                    CaseId = table.Column<long>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    BPCaseCaseId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alerts_BPCases_BPCaseCaseId",
                        column: x => x.BPCaseCaseId,
                        principalTable: "BPCases",
                        principalColumn: "CaseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Alerts_BPs_BPId",
                        column: x => x.BPId,
                        principalTable: "BPs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_BPCaseCaseId",
                table: "Alerts",
                column: "BPCaseCaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_BPId",
                table: "Alerts",
                column: "BPId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alerts");
        }
    }
}
