using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD392.Migrations
{
    /// <inheritdoc />
    public partial class Init15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Order_DiscountId",
                table: "Order",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_discounts_DiscountId",
                table: "Order",
                column: "DiscountId",
                principalTable: "discounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_discounts_DiscountId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_DiscountId",
                table: "Order");
        }
    }
}
