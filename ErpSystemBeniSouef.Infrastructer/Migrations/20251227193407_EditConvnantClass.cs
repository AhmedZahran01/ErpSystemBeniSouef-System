using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpSystemBeniSouef.Infrastructer.Migrations
{
    /// <inheritdoc />
    public partial class EditConvnantClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commissions_customerInvoiceItems_InvoiceItemId",
                table: "Commissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Commissions_products_ProductId",
                table: "Commissions");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Commissions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceItemId",
                table: "Commissions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Commissions_customerInvoiceItems_InvoiceItemId",
                table: "Commissions",
                column: "InvoiceItemId",
                principalTable: "customerInvoiceItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Commissions_products_ProductId",
                table: "Commissions",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commissions_customerInvoiceItems_InvoiceItemId",
                table: "Commissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Commissions_products_ProductId",
                table: "Commissions");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "Commissions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "InvoiceItemId",
                table: "Commissions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Commissions_customerInvoiceItems_InvoiceItemId",
                table: "Commissions",
                column: "InvoiceItemId",
                principalTable: "customerInvoiceItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Commissions_products_ProductId",
                table: "Commissions",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id");
        }
    }
}
