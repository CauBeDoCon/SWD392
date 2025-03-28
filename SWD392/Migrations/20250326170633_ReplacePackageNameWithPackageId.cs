using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD392.Migrations
{
    /// <inheritdoc />
    public partial class ReplacePackageNameWithPackageId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackageName",
                table: "Rooms");

            migrationBuilder.AddColumn<int>(
    name: "PackageId",
    table: "Rooms",
    type: "int",
    nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_PackageId",
                table: "Rooms",
                column: "PackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Packages_PackageId",
                table: "Rooms",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Packages_PackageId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_PackageId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "Rooms");

            migrationBuilder.AddColumn<string>(
                name: "PackageName",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
