using EFModels;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace EFModels.Migrations.MainDBMigration
{
    public partial class FixMainDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BPCases",
                table: "BPCases");

            migrationBuilder.DropColumn(
                name: "Case",
                table: "BPCases");

            migrationBuilder.AlterColumn<long>(
                name: "CaseId",
                table: "BPCases",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BPCases",
                table: "BPCases",
                column: "CaseId");

            migrationBuilder.CreateIndex(
                name: "IX_BPCases_BPId",
                table: "BPCases",
                column: "BPId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BPCases",
                table: "BPCases");

            migrationBuilder.DropIndex(
                name: "IX_BPCases_BPId",
                table: "BPCases");

            migrationBuilder.AlterColumn<long>(
                name: "CaseId",
                table: "BPCases",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long))
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<Case>(
                name: "Case",
                table: "BPCases",
                type: "json",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_BPCases",
                table: "BPCases",
                column: "BPId");
        }
    }
}
