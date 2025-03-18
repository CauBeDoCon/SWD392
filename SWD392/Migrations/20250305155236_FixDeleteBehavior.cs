using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD392.Migrations
{
    /// <inheritdoc />
    public partial class FixDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "skinType",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ResultQuizzes",
                columns: table => new
                {
                    ResultQuizId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quiz1 = table.Column<int>(type: "int", nullable: false),
                    Quiz2 = table.Column<int>(type: "int", nullable: false),
                    Quiz3 = table.Column<int>(type: "int", nullable: false),
                    Quiz4 = table.Column<int>(type: "int", nullable: false),
                    Quiz5 = table.Column<int>(type: "int", nullable: false),
                    Quiz6 = table.Column<int>(type: "int", nullable: false),
                    QuizAnces = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Result = table.Column<int>(type: "int", nullable: false),
                    SkinStatus = table.Column<int>(type: "int", nullable: false),
                    AnceStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultQuizzes", x => x.ResultQuizId);
                    table.ForeignKey(
                        name: "FK_ResultQuizzes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Routines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoutineCategory = table.Column<int>(type: "int", nullable: false),
                    ResultQuizId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Routines_ResultQuizzes_ResultQuizId",
                        column: x => x.ResultQuizId,
                        principalTable: "ResultQuizzes",
                        principalColumn: "ResultQuizId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "routineSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Step = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    RoutineId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_routineSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_routineSteps_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_routineSteps_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_routineSteps_Routines_RoutineId",
                        column: x => x.RoutineId,
                        principalTable: "Routines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResultQuizzes_UserId",
                table: "ResultQuizzes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Routines_ResultQuizId",
                table: "Routines",
                column: "ResultQuizId");

            migrationBuilder.CreateIndex(
                name: "IX_routineSteps_CategoryId",
                table: "routineSteps",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_routineSteps_ProductId",
                table: "routineSteps",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_routineSteps_RoutineId",
                table: "routineSteps",
                column: "RoutineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "routineSteps");

            migrationBuilder.DropTable(
                name: "Routines");

            migrationBuilder.DropTable(
                name: "ResultQuizzes");

            migrationBuilder.DropColumn(
                name: "skinType",
                table: "Product");
        }
    }
}
