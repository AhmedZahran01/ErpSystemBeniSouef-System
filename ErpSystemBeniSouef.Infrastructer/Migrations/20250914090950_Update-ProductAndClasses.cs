using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpSystemBeniSouef.Infrastructer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductAndClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "mainAreas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "categories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mainAreas_CompanyId",
                table: "mainAreas",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_categories_CompanyId",
                table: "categories",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_Company_CompanyId",
                table: "categories",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mainAreas_Company_CompanyId",
                table: "mainAreas",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_Company_CompanyId",
                table: "categories");

            migrationBuilder.DropForeignKey(
                name: "FK_mainAreas_Company_CompanyId",
                table: "mainAreas");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropIndex(
                name: "IX_mainAreas_CompanyId",
                table: "mainAreas");

            migrationBuilder.DropIndex(
                name: "IX_categories_CompanyId",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "mainAreas");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "categories");
        }
    }
}
