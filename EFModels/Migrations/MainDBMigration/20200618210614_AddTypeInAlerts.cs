using Microsoft.EntityFrameworkCore.Migrations;

namespace EFModels.Migrations.MainDBMigration
{
    public partial class AddTypeInAlerts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Alerts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Alerts");
        }
    }
}
