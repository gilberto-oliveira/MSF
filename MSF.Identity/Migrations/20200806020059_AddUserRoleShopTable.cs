using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSF.Identity.Migrations
{
    public partial class AddUserRoleShopTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserRoleShops",
                schema: "System",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    ShopId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleShops", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoleShops_UserRoles_UserId_RoleId",
                        columns: x => new { x.UserId, x.RoleId },
                        principalSchema: "System",
                        principalTable: "UserRoles",
                        principalColumns: new[] { "UserId", "RoleId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleShops_UserId_RoleId",
                schema: "System",
                table: "UserRoleShops",
                columns: new[] { "UserId", "RoleId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRoleShops",
                schema: "System");
        }
    }
}
