using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpSystemBeniSouef.Infrastructer.Migrations
{
    /// <inheritdoc />
    public partial class addOnDeleteCascadeForCustomerInvoices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commissions_customerInvoices_InvoiceId",
                table: "Commissions");

            migrationBuilder.DropForeignKey(
                name: "FK_customerInvoiceItems_customerInvoices_InvoiceId",
                table: "customerInvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_customerInvoices_Customer_CustomerId",
                table: "customerInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Discount_customerInvoices_InvoiceId",
                table: "Discount");

            migrationBuilder.DropForeignKey(
                name: "FK_installmentPlans_customerInvoices_InvoiceId",
                table: "installmentPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyInstallment_customerInvoices_InvoiceId",
                table: "MonthlyInstallment");

            migrationBuilder.AddForeignKey(
                name: "FK_Commissions_customerInvoices_InvoiceId",
                table: "Commissions",
                column: "InvoiceId",
                principalTable: "customerInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_customerInvoiceItems_customerInvoices_InvoiceId",
                table: "customerInvoiceItems",
                column: "InvoiceId",
                principalTable: "customerInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_customerInvoices_Customer_CustomerId",
                table: "customerInvoices",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Discount_customerInvoices_InvoiceId",
                table: "Discount",
                column: "InvoiceId",
                principalTable: "customerInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_installmentPlans_customerInvoices_InvoiceId",
                table: "installmentPlans",
                column: "InvoiceId",
                principalTable: "customerInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyInstallment_customerInvoices_InvoiceId",
                table: "MonthlyInstallment",
                column: "InvoiceId",
                principalTable: "customerInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commissions_customerInvoices_InvoiceId",
                table: "Commissions");

            migrationBuilder.DropForeignKey(
                name: "FK_customerInvoiceItems_customerInvoices_InvoiceId",
                table: "customerInvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_customerInvoices_Customer_CustomerId",
                table: "customerInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Discount_customerInvoices_InvoiceId",
                table: "Discount");

            migrationBuilder.DropForeignKey(
                name: "FK_installmentPlans_customerInvoices_InvoiceId",
                table: "installmentPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyInstallment_customerInvoices_InvoiceId",
                table: "MonthlyInstallment");

            migrationBuilder.AddForeignKey(
                name: "FK_Commissions_customerInvoices_InvoiceId",
                table: "Commissions",
                column: "InvoiceId",
                principalTable: "customerInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_customerInvoiceItems_customerInvoices_InvoiceId",
                table: "customerInvoiceItems",
                column: "InvoiceId",
                principalTable: "customerInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_customerInvoices_Customer_CustomerId",
                table: "customerInvoices",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Discount_customerInvoices_InvoiceId",
                table: "Discount",
                column: "InvoiceId",
                principalTable: "customerInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_installmentPlans_customerInvoices_InvoiceId",
                table: "installmentPlans",
                column: "InvoiceId",
                principalTable: "customerInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyInstallment_customerInvoices_InvoiceId",
                table: "MonthlyInstallment",
                column: "InvoiceId",
                principalTable: "customerInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
