using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpSystemBeniSouef.Infrastructer.Migrations
{
    /// <inheritdoc />
    public partial class editCheckConnect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "invoices",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "invoices");
        }
    }
}
