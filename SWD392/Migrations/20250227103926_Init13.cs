using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD392.Migrations
{
    /// <inheritdoc />
    public partial class Init13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discount_discountCategories_DiscountCategoryId",
                table: "Discount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Discount",
                table: "Discount");

            migrationBuilder.RenameTable(
                name: "Discount",
                newName: "discounts");

            migrationBuilder.RenameIndex(
                name: "IX_Discount_DiscountCategoryId",
                table: "discounts",
                newName: "IX_discounts_DiscountCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_discounts",
                table: "discounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_discounts_discountCategories_DiscountCategoryId",
                table: "discounts",
                column: "DiscountCategoryId",
                principalTable: "discountCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_discounts_discountCategories_DiscountCategoryId",
                table: "discounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_discounts",
                table: "discounts");

            migrationBuilder.RenameTable(
                name: "discounts",
                newName: "Discount");

            migrationBuilder.RenameIndex(
                name: "IX_discounts_DiscountCategoryId",
                table: "Discount",
                newName: "IX_Discount_DiscountCategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Discount",
                table: "Discount",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Discount_discountCategories_DiscountCategoryId",
                table: "Discount",
                column: "DiscountCategoryId",
                principalTable: "discountCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
