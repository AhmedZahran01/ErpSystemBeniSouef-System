using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpSystemBeniSouef.Infrastructer.Migrations
{
    /// <inheritdoc />
    public partial class Update6ProductAndClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mainAreas_company_CompanyId",
                table: "mainAreas");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "mainAreas",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_mainAreas_company_CompanyId",
                table: "mainAreas",
                column: "CompanyId",
                principalTable: "company",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mainAreas_company_CompanyId",
                table: "mainAreas");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "mainAreas",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_mainAreas_company_CompanyId",
                table: "mainAreas",
                column: "CompanyId",
                principalTable: "company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
