using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpSystemBeniSouef.Infrastructer.Migrations
{
    /// <inheritdoc />
    public partial class Update4ProductAndClasses : Migration
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_Company",
                table: "Company");

            migrationBuilder.RenameTable(
                name: "Company",
                newName: "company");

            migrationBuilder.AddPrimaryKey(
                name: "PK_company",
                table: "company",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_categories_company_CompanyId",
                table: "categories",
                column: "CompanyId",
                principalTable: "company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mainAreas_company_CompanyId",
                table: "mainAreas",
                column: "CompanyId",
                principalTable: "company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categories_company_CompanyId",
                table: "categories");

            migrationBuilder.DropForeignKey(
                name: "FK_mainAreas_company_CompanyId",
                table: "mainAreas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_company",
                table: "company");

            migrationBuilder.RenameTable(
                name: "company",
                newName: "Company");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Company",
                table: "Company",
                column: "Id");

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
