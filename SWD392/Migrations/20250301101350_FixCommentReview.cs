using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD392.Migrations
{
    /// <inheritdoc />
    public partial class FixCommentReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Review_ReviewId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_OrderDetails_OrderDetailId",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Review_OrderDetailId",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Comment_ReviewId",
                table: "Comment");

            migrationBuilder.AlterColumn<int>(
                name: "OrderDetailId",
                table: "Review",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ReviewId",
                table: "Comment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Review_OrderDetailId",
                table: "Review",
                column: "OrderDetailId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ReviewId",
                table: "Comment",
                column: "ReviewId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Review_ReviewId",
                table: "Comment",
                column: "ReviewId",
                principalTable: "Review",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_OrderDetails_OrderDetailId",
                table: "Review",
                column: "OrderDetailId",
                principalTable: "OrderDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Review_ReviewId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_OrderDetails_OrderDetailId",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Review_OrderDetailId",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Comment_ReviewId",
                table: "Comment");

            migrationBuilder.AlterColumn<int>(
                name: "OrderDetailId",
                table: "Review",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewId",
                table: "Comment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Review_OrderDetailId",
                table: "Review",
                column: "OrderDetailId",
                unique: true,
                filter: "[OrderDetailId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ReviewId",
                table: "Comment",
                column: "ReviewId",
                unique: true,
                filter: "[ReviewId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Review_ReviewId",
                table: "Comment",
                column: "ReviewId",
                principalTable: "Review",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_OrderDetails_OrderDetailId",
                table: "Review",
                column: "OrderDetailId",
                principalTable: "OrderDetails",
                principalColumn: "Id");
        }
    }
}
