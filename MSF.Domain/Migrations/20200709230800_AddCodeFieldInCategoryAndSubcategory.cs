using Microsoft.EntityFrameworkCore.Migrations;

namespace MSF.Domain.Migrations
{
    public partial class AddCodeFieldInCategoryAndSubcategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "MSF",
                table: "Subcategories",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "MSF",
                table: "Categories",
                maxLength: 5,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                schema: "MSF",
                table: "Subcategories");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "MSF",
                table: "Categories");
        }
    }
}
