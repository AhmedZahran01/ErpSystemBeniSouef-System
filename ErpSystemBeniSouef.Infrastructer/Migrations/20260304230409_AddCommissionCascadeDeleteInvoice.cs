using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpSystemBeniSouef.Infrastructer.Migrations
{
    /// <inheritdoc />
    public partial class AddCommissionCascadeDeleteInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commissions_customerInvoices_InvoiceId",
                table: "Commissions");

            migrationBuilder.AddForeignKey(
                name: "FK_Commissions_customerInvoices_InvoiceId",
                table: "Commissions",
                column: "InvoiceId",
                principalTable: "customerInvoices",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commissions_customerInvoices_InvoiceId",
                table: "Commissions");

            migrationBuilder.AddForeignKey(
                name: "FK_Commissions_customerInvoices_InvoiceId",
                table: "Commissions",
                column: "InvoiceId",
                principalTable: "customerInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
