using Microsoft.EntityFrameworkCore.Migrations;

namespace MSF.Domain.Migrations
{
    public partial class RemoveOperationsFromWorkCenterTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operations_WorkCenters_WorkCenterId",
                schema: "MSF",
                table: "Operations");

            migrationBuilder.DropIndex(
                name: "IX_Operations_WorkCenterId",
                schema: "MSF",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "WorkCenterId",
                schema: "MSF",
                table: "Operations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkCenterId",
                schema: "MSF",
                table: "Operations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Operations_WorkCenterId",
                schema: "MSF",
                table: "Operations",
                column: "WorkCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_WorkCenters_WorkCenterId",
                schema: "MSF",
                table: "Operations",
                column: "WorkCenterId",
                principalSchema: "MSF",
                principalTable: "WorkCenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
