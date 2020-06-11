using EFModels;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFModels.Migrations.MainDBMigration
{
    public partial class addStandartCase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<StandartCase>(
                name: "StandartCase",
                table: "BPs",
                type: "json",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StandartCase",
                table: "BPs");
        }
    }
}
