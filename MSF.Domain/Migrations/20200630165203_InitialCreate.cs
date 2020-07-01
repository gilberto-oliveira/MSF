using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MSF.Domain.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Security");

            migrationBuilder.EnsureSchema(
                name: "MSF");

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "MSF",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shops",
                schema: "MSF",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<string>(maxLength: 10, nullable: true),
                    Description = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                schema: "MSF",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Initials = table.Column<string>(maxLength: 3, nullable: true),
                    Name = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Audits",
                schema: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false, defaultValueSql: "GETDATE()"),
                    TableName = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    KeyValues = table.Column<string>(nullable: true),
                    OldValues = table.Column<string>(nullable: true),
                    NewValues = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subcategories",
                schema: "MSF",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subcategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subcategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "MSF",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkCenters",
                schema: "MSF",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ShopId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 30, nullable: true),
                    Code = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkCenters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkCenters_Shops_ShopId",
                        column: x => x.ShopId,
                        principalSchema: "MSF",
                        principalTable: "Shops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                schema: "MSF",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StateId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 30, nullable: true),
                    Code = table.Column<string>(maxLength: 120, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Providers_States_StateId",
                        column: x => x.StateId,
                        principalSchema: "MSF",
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "MSF",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SubcategoryId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 256, nullable: true),
                    Profit = table.Column<decimal>(type: "decimal (18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Subcategories_SubcategoryId",
                        column: x => x.SubcategoryId,
                        principalSchema: "MSF",
                        principalTable: "Subcategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkCenterControls",
                schema: "MSF",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    WorkCenterId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    FinalDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkCenterControls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkCenterControls_WorkCenters_WorkCenterId",
                        column: x => x.WorkCenterId,
                        principalSchema: "MSF",
                        principalTable: "WorkCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stocks",
                schema: "MSF",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(nullable: false),
                    ProviderId = table.Column<int>(nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal (18,4)", nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Amount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stocks_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "MSF",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stocks_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalSchema: "MSF",
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                schema: "MSF",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    WorkCenterControlId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    ProviderId = table.Column<int>(nullable: false),
                    Type = table.Column<string>(maxLength: 2, nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal (18,4)", nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    WorkCenterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Operations_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "MSF",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Operations_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalSchema: "MSF",
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Operations_WorkCenterControls_WorkCenterControlId",
                        column: x => x.WorkCenterControlId,
                        principalSchema: "MSF",
                        principalTable: "WorkCenterControls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Operations_WorkCenters_WorkCenterId",
                        column: x => x.WorkCenterId,
                        principalSchema: "MSF",
                        principalTable: "WorkCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Operations_ProductId",
                schema: "MSF",
                table: "Operations",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_ProviderId",
                schema: "MSF",
                table: "Operations",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_WorkCenterControlId",
                schema: "MSF",
                table: "Operations",
                column: "WorkCenterControlId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_WorkCenterId",
                schema: "MSF",
                table: "Operations",
                column: "WorkCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubcategoryId",
                schema: "MSF",
                table: "Products",
                column: "SubcategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Providers_StateId",
                schema: "MSF",
                table: "Providers",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ProductId",
                schema: "MSF",
                table: "Stocks",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Stocks_ProviderId",
                schema: "MSF",
                table: "Stocks",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Subcategories_CategoryId",
                schema: "MSF",
                table: "Subcategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkCenterControls_WorkCenterId",
                schema: "MSF",
                table: "WorkCenterControls",
                column: "WorkCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkCenters_ShopId",
                schema: "MSF",
                table: "WorkCenters",
                column: "ShopId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Operations",
                schema: "MSF");

            migrationBuilder.DropTable(
                name: "Stocks",
                schema: "MSF");

            migrationBuilder.DropTable(
                name: "Audits",
                schema: "Security");

            migrationBuilder.DropTable(
                name: "WorkCenterControls",
                schema: "MSF");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "MSF");

            migrationBuilder.DropTable(
                name: "Providers",
                schema: "MSF");

            migrationBuilder.DropTable(
                name: "WorkCenters",
                schema: "MSF");

            migrationBuilder.DropTable(
                name: "Subcategories",
                schema: "MSF");

            migrationBuilder.DropTable(
                name: "States",
                schema: "MSF");

            migrationBuilder.DropTable(
                name: "Shops",
                schema: "MSF");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "MSF");
        }
    }
}
