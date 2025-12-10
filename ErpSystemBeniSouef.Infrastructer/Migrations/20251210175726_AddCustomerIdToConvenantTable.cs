using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpSystemBeniSouef.Infrastructer.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerIdToConvenantTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Covenant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Covenant_CustomerId",
                table: "Covenant",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Covenant_Customer_CustomerId",
                table: "Covenant",
                column: "CustomerId",
                principalTable: "Customer",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Covenant_Customer_CustomerId",
                table: "Covenant");

            migrationBuilder.DropIndex(
                name: "IX_Covenant_CustomerId",
                table: "Covenant");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Covenant");
        }
    }
}
