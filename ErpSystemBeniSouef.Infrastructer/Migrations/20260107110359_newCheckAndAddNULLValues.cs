using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpSystemBeniSouef.Infrastructer.Migrations
{
    /// <inheritdoc />
    public partial class newCheckAndAddNULLValues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_collectors_CollectorId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_representatives_RepresentativeId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_subAreas_SubAreaId",
                table: "Customer");

            migrationBuilder.AlterColumn<int>(
                name: "SubAreaId",
                table: "Customer",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RepresentativeId",
                table: "Customer",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CollectorId",
                table: "Customer",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_collectors_CollectorId",
                table: "Customer",
                column: "CollectorId",
                principalTable: "collectors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_representatives_RepresentativeId",
                table: "Customer",
                column: "RepresentativeId",
                principalTable: "representatives",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_subAreas_SubAreaId",
                table: "Customer",
                column: "SubAreaId",
                principalTable: "subAreas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_collectors_CollectorId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_representatives_RepresentativeId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_subAreas_SubAreaId",
                table: "Customer");

            migrationBuilder.AlterColumn<int>(
                name: "SubAreaId",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RepresentativeId",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CollectorId",
                table: "Customer",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_collectors_CollectorId",
                table: "Customer",
                column: "CollectorId",
                principalTable: "collectors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_representatives_RepresentativeId",
                table: "Customer",
                column: "RepresentativeId",
                principalTable: "representatives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_subAreas_SubAreaId",
                table: "Customer",
                column: "SubAreaId",
                principalTable: "subAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
