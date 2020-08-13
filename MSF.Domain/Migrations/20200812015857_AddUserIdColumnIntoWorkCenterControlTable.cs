using Microsoft.EntityFrameworkCore.Migrations;

namespace MSF.Domain.Migrations
{
    public partial class AddUserIdColumnIntoWorkCenterControlTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                schema: "MSF",
                table: "WorkCenterControls",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "MSF",
                table: "WorkCenterControls");
        }
    }
}
