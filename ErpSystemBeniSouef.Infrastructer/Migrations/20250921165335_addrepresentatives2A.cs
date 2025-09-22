using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErpSystemBeniSouef.Infrastructer.Migrations
{
    /// <inheritdoc />
    public partial class addrepresentatives2A : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "representatives",
                newName: "UserNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserNumber",
                table: "representatives",
                newName: "Username");
        }
    }
}
