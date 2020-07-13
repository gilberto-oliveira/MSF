using Microsoft.EntityFrameworkCore.Migrations;

namespace MSF.Domain.Migrations
{
    public partial class AddUniqueIndexInCodeColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "MSF",
                table: "Subcategories",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "MSF",
                table: "Subcategories",
                maxLength: 4,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 4,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "MSF",
                table: "Categories",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "MSF",
                table: "Categories",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 5,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subcategories_Code",
                schema: "MSF",
                table: "Subcategories",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Code",
                schema: "MSF",
                table: "Categories",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Subcategories_Code",
                schema: "MSF",
                table: "Subcategories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Code",
                schema: "MSF",
                table: "Categories");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "MSF",
                table: "Subcategories",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "MSF",
                table: "Subcategories",
                maxLength: 4,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 4);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "MSF",
                table: "Categories",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                schema: "MSF",
                table: "Categories",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 5);
        }
    }
}
