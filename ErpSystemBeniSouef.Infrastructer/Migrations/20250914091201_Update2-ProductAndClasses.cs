using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpSystemBeniSouef.Infrastructer.Migrations
{
    /// <inheritdoc />
    public partial class Update2ProductAndClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_Company_CompanyId",
                table: "categories");

            migrationBuilder.DropForeignKey(
                name: "FK_mainAreas_Company_CompanyId",
                table: "mainAreas");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.CreateTable(
                name: "OurCompany",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OurCompany", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_categories_OurCompany_CompanyId",
                table: "categories",
                column: "CompanyId",
                principalTable: "OurCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mainAreas_OurCompany_CompanyId",
                table: "mainAreas",
                column: "CompanyId",
                principalTable: "OurCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_OurCompany_CompanyId",
                table: "categories");

            migrationBuilder.DropForeignKey(
                name: "FK_mainAreas_OurCompany_CompanyId",
                table: "mainAreas");

            migrationBuilder.DropTable(
                name: "OurCompany");

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

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
    }
}
