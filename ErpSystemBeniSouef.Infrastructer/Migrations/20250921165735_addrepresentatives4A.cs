using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpSystemBeniSouef.Infrastructer.Migrations
{
    /// <inheritdoc />
    public partial class addrepresentatives4A : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "representatives");

            migrationBuilder.AddColumn<int>(
                name: "UserNumber",
                table: "representatives",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserNumber",
                table: "representatives");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "representatives",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
