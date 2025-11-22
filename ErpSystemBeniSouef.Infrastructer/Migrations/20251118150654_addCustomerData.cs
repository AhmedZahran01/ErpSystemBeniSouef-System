using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpSystemBeniSouef.Infrastructer.Migrations
{
    /// <inheritdoc />
    public partial class addCustomerData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cashCstomerInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubAreaId = table.Column<int>(type: "int", nullable: false),
                    RepresentativeId = table.Column<int>(type: "int", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cashCstomerInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cashCstomerInvoices_representatives_RepresentativeId",
                        column: x => x.RepresentativeId,
                        principalTable: "representatives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cashCstomerInvoices_subAreas_SubAreaId",
                        column: x => x.SubAreaId,
                        principalTable: "subAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectionBatch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CollectorId = table.Column<int>(type: "int", nullable: false),
                    MonthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAssignedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CarriedOverAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionBatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollectionBatch_collectors_CollectorId",
                        column: x => x.CollectorId,
                        principalTable: "collectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerNumber = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SaleDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstInvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NationalNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Deposit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubAreaId = table.Column<int>(type: "int", nullable: false),
                    CollectorId = table.Column<int>(type: "int", nullable: false),
                    RepresentativeId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_collectors_CollectorId",
                        column: x => x.CollectorId,
                        principalTable: "collectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customer_representatives_RepresentativeId",
                        column: x => x.RepresentativeId,
                        principalTable: "representatives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customer_subAreas_SubAreaId",
                        column: x => x.SubAreaId,
                        principalTable: "subAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CashCustomerInvoiceItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CashCustomerInvoiceId = table.Column<int>(type: "int", nullable: true),
                    cashCstomerInvoiceId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashCustomerInvoiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashCustomerInvoiceItems_cashCstomerInvoices_cashCstomerInvoiceId",
                        column: x => x.cashCstomerInvoiceId,
                        principalTable: "cashCstomerInvoices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CashCustomerInvoiceItems_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customerInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    InvoiceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customerInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_customerInvoices_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customerInvoiceItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customerInvoiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_customerInvoiceItems_customerInvoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "customerInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_customerInvoiceItems_products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "installmentPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    NumberOfMonths = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_installmentPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_installmentPlans_customerInvoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "customerInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyInstallment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    CollectorId = table.Column<int>(type: "int", nullable: false),
                    MonthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CollectedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    IsDelayed = table.Column<bool>(type: "bit", nullable: false),
                    CollectionBatchId = table.Column<int>(type: "int", nullable: true),
                    CollectorId1 = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyInstallment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonthlyInstallment_CollectionBatch_CollectionBatchId",
                        column: x => x.CollectionBatchId,
                        principalTable: "CollectionBatch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MonthlyInstallment_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MonthlyInstallment_collectors_CollectorId",
                        column: x => x.CollectorId,
                        principalTable: "collectors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MonthlyInstallment_collectors_CollectorId1",
                        column: x => x.CollectorId1,
                        principalTable: "collectors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MonthlyInstallment_customerInvoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "customerInvoices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CollectionEntry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MonthlyInstallmentId = table.Column<int>(type: "int", nullable: false),
                    CollectionBatchId = table.Column<int>(type: "int", nullable: true),
                    CollectionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollectionEntry_CollectionBatch_CollectionBatchId",
                        column: x => x.CollectionBatchId,
                        principalTable: "CollectionBatch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CollectionEntry_MonthlyInstallment_MonthlyInstallmentId",
                        column: x => x.MonthlyInstallmentId,
                        principalTable: "MonthlyInstallment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cashCstomerInvoices_RepresentativeId",
                table: "cashCstomerInvoices",
                column: "RepresentativeId");

            migrationBuilder.CreateIndex(
                name: "IX_cashCstomerInvoices_SubAreaId",
                table: "cashCstomerInvoices",
                column: "SubAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_CashCustomerInvoiceItems_cashCstomerInvoiceId",
                table: "CashCustomerInvoiceItems",
                column: "cashCstomerInvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CashCustomerInvoiceItems_ProductId",
                table: "CashCustomerInvoiceItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionBatch_CollectorId",
                table: "CollectionBatch",
                column: "CollectorId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionEntry_CollectionBatchId",
                table: "CollectionEntry",
                column: "CollectionBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionEntry_MonthlyInstallmentId",
                table: "CollectionEntry",
                column: "MonthlyInstallmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CollectorId",
                table: "Customer",
                column: "CollectorId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_RepresentativeId",
                table: "Customer",
                column: "RepresentativeId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_SubAreaId",
                table: "Customer",
                column: "SubAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_customerInvoiceItems_InvoiceId",
                table: "customerInvoiceItems",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_customerInvoiceItems_ProductId",
                table: "customerInvoiceItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_customerInvoices_CustomerId",
                table: "customerInvoices",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_installmentPlans_InvoiceId",
                table: "installmentPlans",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyInstallment_CollectionBatchId",
                table: "MonthlyInstallment",
                column: "CollectionBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyInstallment_CollectorId",
                table: "MonthlyInstallment",
                column: "CollectorId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyInstallment_CollectorId1",
                table: "MonthlyInstallment",
                column: "CollectorId1");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyInstallment_CustomerId",
                table: "MonthlyInstallment",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyInstallment_InvoiceId",
                table: "MonthlyInstallment",
                column: "InvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashCustomerInvoiceItems");

            migrationBuilder.DropTable(
                name: "CollectionEntry");

            migrationBuilder.DropTable(
                name: "customerInvoiceItems");

            migrationBuilder.DropTable(
                name: "installmentPlans");

            migrationBuilder.DropTable(
                name: "cashCstomerInvoices");

            migrationBuilder.DropTable(
                name: "MonthlyInstallment");

            migrationBuilder.DropTable(
                name: "CollectionBatch");

            migrationBuilder.DropTable(
                name: "customerInvoices");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
