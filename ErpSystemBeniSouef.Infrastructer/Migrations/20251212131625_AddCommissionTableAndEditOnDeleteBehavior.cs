using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpSystemBeniSouef.Infrastructer.Migrations
{
    /// <inheritdoc />
    public partial class AddCommissionTableAndEditOnDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_Roles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_Roles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_cashCstomerInvoices_representatives_RepresentativeId",
                table: "cashCstomerInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_cashCstomerInvoices_subAreas_SubAreaId",
                table: "cashCstomerInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_CashCustomerInvoiceItems_products_ProductId",
                table: "CashCustomerInvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CollectionBatch_collectors_CollectorId",
                table: "CollectionBatch");

            migrationBuilder.DropForeignKey(
                name: "FK_CollectionEntry_MonthlyInstallment_MonthlyInstallmentId",
                table: "CollectionEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_Covenant_Customer_CustomerId",
                table: "Covenant");

            migrationBuilder.DropForeignKey(
                name: "FK_Covenant_representatives_RepresentativeId",
                table: "Covenant");

            migrationBuilder.DropForeignKey(
                name: "FK_CovenantProduct_Covenant_CovenantId",
                table: "CovenantProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_CovenantProduct_products_ProductId",
                table: "CovenantProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_collectors_CollectorId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_representatives_RepresentativeId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_subAreas_SubAreaId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_customerInvoiceItems_customerInvoices_InvoiceId",
                table: "customerInvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_customerInvoiceItems_products_ProductId",
                table: "customerInvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_customerInvoices_Customer_CustomerId",
                table: "customerInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_installmentPlans_customerInvoices_InvoiceId",
                table: "installmentPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_invoiceItems_invoices_InvoiceId",
                table: "invoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_invoiceItems_products_ProductId",
                table: "invoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyInstallment_Customer_CustomerId",
                table: "MonthlyInstallment");

            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyInstallment_collectors_CollectorId",
                table: "MonthlyInstallment");

            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyInstallment_collectors_CollectorId1",
                table: "MonthlyInstallment");

            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyInstallment_customerInvoices_InvoiceId",
                table: "MonthlyInstallment");

            migrationBuilder.DropForeignKey(
                name: "FK_products_categories_CategoryId",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "FK_products_company_CompanyId",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierAccount_suppliers_SupplierId",
                table: "SupplierAccount");

            migrationBuilder.DropIndex(
                name: "IX_MonthlyInstallment_CollectorId1",
                table: "MonthlyInstallment");

            migrationBuilder.DropColumn(
                name: "CollectorId1",
                table: "MonthlyInstallment");

            migrationBuilder.CreateTable(
                name: "Commissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepresentativeId = table.Column<int>(type: "int", nullable: false),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    InvoiceItemId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    TotalCommission = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MonthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CommissionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DeductedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeducted = table.Column<bool>(type: "bit", nullable: false),
                    DeductedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commissions_customerInvoiceItems_InvoiceItemId",
                        column: x => x.InvoiceItemId,
                        principalTable: "customerInvoiceItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Commissions_customerInvoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "customerInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Commissions_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Commissions_representatives_RepresentativeId",
                        column: x => x.RepresentativeId,
                        principalTable: "representatives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_InvoiceId",
                table: "Commissions",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_InvoiceItemId",
                table: "Commissions",
                column: "InvoiceItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_ProductId",
                table: "Commissions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Commissions_RepresentativeId",
                table: "Commissions",
                column: "RepresentativeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_Roles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_Roles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_cashCstomerInvoices_representatives_RepresentativeId",
                table: "cashCstomerInvoices",
                column: "RepresentativeId",
                principalTable: "representatives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_cashCstomerInvoices_subAreas_SubAreaId",
                table: "cashCstomerInvoices",
                column: "SubAreaId",
                principalTable: "subAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CashCustomerInvoiceItems_products_ProductId",
                table: "CashCustomerInvoiceItems",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CollectionBatch_collectors_CollectorId",
                table: "CollectionBatch",
                column: "CollectorId",
                principalTable: "collectors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CollectionEntry_MonthlyInstallment_MonthlyInstallmentId",
                table: "CollectionEntry",
                column: "MonthlyInstallmentId",
                principalTable: "MonthlyInstallment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Covenant_Customer_CustomerId",
                table: "Covenant",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Covenant_representatives_RepresentativeId",
                table: "Covenant",
                column: "RepresentativeId",
                principalTable: "representatives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CovenantProduct_Covenant_CovenantId",
                table: "CovenantProduct",
                column: "CovenantId",
                principalTable: "Covenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CovenantProduct_products_ProductId",
                table: "CovenantProduct",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_customerInvoiceItems_customerInvoices_InvoiceId",
                table: "customerInvoiceItems",
                column: "InvoiceId",
                principalTable: "customerInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_customerInvoiceItems_products_ProductId",
                table: "customerInvoiceItems",
                column: "ProductId",
                principalTable: "products",
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
                name: "FK_installmentPlans_customerInvoices_InvoiceId",
                table: "installmentPlans",
                column: "InvoiceId",
                principalTable: "customerInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_invoiceItems_invoices_InvoiceId",
                table: "invoiceItems",
                column: "InvoiceId",
                principalTable: "invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_invoiceItems_products_ProductId",
                table: "invoiceItems",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyInstallment_Customer_CustomerId",
                table: "MonthlyInstallment",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyInstallment_collectors_CollectorId",
                table: "MonthlyInstallment",
                column: "CollectorId",
                principalTable: "collectors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyInstallment_customerInvoices_InvoiceId",
                table: "MonthlyInstallment",
                column: "InvoiceId",
                principalTable: "customerInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_products_categories_CategoryId",
                table: "products",
                column: "CategoryId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_products_company_CompanyId",
                table: "products",
                column: "CompanyId",
                principalTable: "company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierAccount_suppliers_SupplierId",
                table: "SupplierAccount",
                column: "SupplierId",
                principalTable: "suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_Roles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_Roles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_cashCstomerInvoices_representatives_RepresentativeId",
                table: "cashCstomerInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_cashCstomerInvoices_subAreas_SubAreaId",
                table: "cashCstomerInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_CashCustomerInvoiceItems_products_ProductId",
                table: "CashCustomerInvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CollectionBatch_collectors_CollectorId",
                table: "CollectionBatch");

            migrationBuilder.DropForeignKey(
                name: "FK_CollectionEntry_MonthlyInstallment_MonthlyInstallmentId",
                table: "CollectionEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_Covenant_Customer_CustomerId",
                table: "Covenant");

            migrationBuilder.DropForeignKey(
                name: "FK_Covenant_representatives_RepresentativeId",
                table: "Covenant");

            migrationBuilder.DropForeignKey(
                name: "FK_CovenantProduct_Covenant_CovenantId",
                table: "CovenantProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_CovenantProduct_products_ProductId",
                table: "CovenantProduct");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_collectors_CollectorId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_representatives_RepresentativeId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_subAreas_SubAreaId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_customerInvoiceItems_customerInvoices_InvoiceId",
                table: "customerInvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_customerInvoiceItems_products_ProductId",
                table: "customerInvoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_customerInvoices_Customer_CustomerId",
                table: "customerInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_installmentPlans_customerInvoices_InvoiceId",
                table: "installmentPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_invoiceItems_invoices_InvoiceId",
                table: "invoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_invoiceItems_products_ProductId",
                table: "invoiceItems");

            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyInstallment_Customer_CustomerId",
                table: "MonthlyInstallment");

            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyInstallment_collectors_CollectorId",
                table: "MonthlyInstallment");

            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyInstallment_customerInvoices_InvoiceId",
                table: "MonthlyInstallment");

            migrationBuilder.DropForeignKey(
                name: "FK_products_categories_CategoryId",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "FK_products_company_CompanyId",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplierAccount_suppliers_SupplierId",
                table: "SupplierAccount");

            migrationBuilder.DropTable(
                name: "Commissions");

            migrationBuilder.AddColumn<int>(
                name: "CollectorId1",
                table: "MonthlyInstallment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyInstallment_CollectorId1",
                table: "MonthlyInstallment",
                column: "CollectorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_Roles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_Roles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cashCstomerInvoices_representatives_RepresentativeId",
                table: "cashCstomerInvoices",
                column: "RepresentativeId",
                principalTable: "representatives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cashCstomerInvoices_subAreas_SubAreaId",
                table: "cashCstomerInvoices",
                column: "SubAreaId",
                principalTable: "subAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CashCustomerInvoiceItems_products_ProductId",
                table: "CashCustomerInvoiceItems",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CollectionBatch_collectors_CollectorId",
                table: "CollectionBatch",
                column: "CollectorId",
                principalTable: "collectors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CollectionEntry_MonthlyInstallment_MonthlyInstallmentId",
                table: "CollectionEntry",
                column: "MonthlyInstallmentId",
                principalTable: "MonthlyInstallment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Covenant_Customer_CustomerId",
                table: "Covenant",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Covenant_representatives_RepresentativeId",
                table: "Covenant",
                column: "RepresentativeId",
                principalTable: "representatives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CovenantProduct_Covenant_CovenantId",
                table: "CovenantProduct",
                column: "CovenantId",
                principalTable: "Covenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CovenantProduct_products_ProductId",
                table: "CovenantProduct",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_collectors_CollectorId",
                table: "Customer",
                column: "CollectorId",
                principalTable: "collectors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_representatives_RepresentativeId",
                table: "Customer",
                column: "RepresentativeId",
                principalTable: "representatives",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_subAreas_SubAreaId",
                table: "Customer",
                column: "SubAreaId",
                principalTable: "subAreas",
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
                name: "FK_customerInvoiceItems_products_ProductId",
                table: "customerInvoiceItems",
                column: "ProductId",
                principalTable: "products",
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
                name: "FK_installmentPlans_customerInvoices_InvoiceId",
                table: "installmentPlans",
                column: "InvoiceId",
                principalTable: "customerInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_invoiceItems_invoices_InvoiceId",
                table: "invoiceItems",
                column: "InvoiceId",
                principalTable: "invoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_invoiceItems_products_ProductId",
                table: "invoiceItems",
                column: "ProductId",
                principalTable: "products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyInstallment_Customer_CustomerId",
                table: "MonthlyInstallment",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyInstallment_collectors_CollectorId",
                table: "MonthlyInstallment",
                column: "CollectorId",
                principalTable: "collectors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyInstallment_collectors_CollectorId1",
                table: "MonthlyInstallment",
                column: "CollectorId1",
                principalTable: "collectors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyInstallment_customerInvoices_InvoiceId",
                table: "MonthlyInstallment",
                column: "InvoiceId",
                principalTable: "customerInvoices",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_products_categories_CategoryId",
                table: "products",
                column: "CategoryId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_products_company_CompanyId",
                table: "products",
                column: "CompanyId",
                principalTable: "company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplierAccount_suppliers_SupplierId",
                table: "SupplierAccount",
                column: "SupplierId",
                principalTable: "suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
