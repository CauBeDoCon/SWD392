using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD392.Migrations
{
    /// <inheritdoc />
    public partial class reviewtoorderdetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderDetailId",
                table: "Review",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Review_OrderDetailId",
                table: "Review",
                column: "OrderDetailId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_OrderDetails_OrderDetailId",
                table: "Review",
                column: "OrderDetailId",
                principalTable: "OrderDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_OrderDetails_OrderDetailId",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Review_OrderDetailId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "OrderDetailId",
                table: "Review");
        }
    }
}
