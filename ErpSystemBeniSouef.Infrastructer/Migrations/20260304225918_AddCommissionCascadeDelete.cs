using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpSystemBeniSouef.Infrastructer.Migrations
{
    /// <inheritdoc />
    public partial class AddCommissionCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commissions_customerInvoiceItems_InvoiceItemId",
                table: "Commissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Commissions_customerInvoices_InvoiceId",
                table: "Commissions");

            migrationBuilder.AddForeignKey(
                name: "FK_Commissions_customerInvoiceItems_InvoiceItemId",
                table: "Commissions",
                column: "InvoiceItemId",
                principalTable: "customerInvoiceItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Commissions_customerInvoices_InvoiceId",
                table: "Commissions",
                column: "InvoiceId",
                principalTable: "customerInvoices",
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
                name: "FK_Commissions_customerInvoices_InvoiceId",
                table: "Commissions");

            migrationBuilder.AddForeignKey(
                name: "FK_Commissions_customerInvoiceItems_InvoiceItemId",
                table: "Commissions",
                column: "InvoiceItemId",
                principalTable: "customerInvoiceItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Commissions_customerInvoices_InvoiceId",
                table: "Commissions",
                column: "InvoiceId",
                principalTable: "customerInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
