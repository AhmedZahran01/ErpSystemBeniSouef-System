using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpSystemBeniSouef.Infrastructer.Migrations
{
    /// <inheritdoc />
    public partial class newReturnData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Covenant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CovenantDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MonthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RepresentativeId = table.Column<int>(type: "int", nullable: false),
                    CovenantType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Covenant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Covenant_representatives_RepresentativeId",
                        column: x => x.RepresentativeId,
                        principalTable: "representatives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CovenantProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CovenantId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CovenantProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CovenantProduct_Covenant_CovenantId",
                        column: x => x.CovenantId,
                        principalTable: "Covenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CovenantProduct_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Covenant_RepresentativeId",
                table: "Covenant",
                column: "RepresentativeId");

            migrationBuilder.CreateIndex(
                name: "IX_CovenantProduct_CovenantId",
                table: "CovenantProduct",
                column: "CovenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CovenantProduct_ProductId",
                table: "CovenantProduct",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CovenantProduct");

            migrationBuilder.DropTable(
                name: "Covenant");
        }
    }
}
