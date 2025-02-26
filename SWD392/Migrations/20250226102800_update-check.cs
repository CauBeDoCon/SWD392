using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD392.Migrations
{
    /// <inheritdoc />
    public partial class updatecheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Product",
                newName: "StockRemaining");

            migrationBuilder.AddColumn<int>(
                name: "InitialStock",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialStock",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "StockRemaining",
                table: "Product",
                newName: "Quantity");
        }
    }
}
