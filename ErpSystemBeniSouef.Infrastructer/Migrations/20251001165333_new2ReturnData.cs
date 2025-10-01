using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpSystemBeniSouef.Infrastructer.Migrations
{
    /// <inheritdoc />
    public partial class new2ReturnData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "SupplierAccount",
                newName: "Notes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "SupplierAccount",
                newName: "Description");
        }
    }
}
