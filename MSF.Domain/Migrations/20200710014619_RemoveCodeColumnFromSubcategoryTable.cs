using Microsoft.EntityFrameworkCore.Migrations;

namespace MSF.Domain.Migrations
{
    public partial class RemoveCodeColumnFromSubcategoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subcategories_Code",
                schema: "MSF",
                table: "Subcategories");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "MSF",
                table: "Subcategories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "MSF",
                table: "Subcategories",
                maxLength: 4,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Subcategories_Code",
                schema: "MSF",
                table: "Subcategories",
                column: "Code",
                unique: true);
        }
    }
}
