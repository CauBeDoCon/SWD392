using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD392.Migrations
{
    /// <inheritdoc />
    public partial class FixBookingCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_DoctorId",
                table: "Bookings",
                column: "DoctorId");

            

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_DoctorId",
                table: "Bookings",
                column: "DoctorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_DoctorId",
                table: "Bookings");

           

            migrationBuilder.DropIndex(
                name: "IX_Bookings_DoctorId",
                table: "Bookings");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
