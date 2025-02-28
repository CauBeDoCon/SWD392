using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD392.Migrations
{
    /// <inheritdoc />
    public partial class AddQuiz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResultQuiz",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quiz1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quiz2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quiz3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quiz4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quiz5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quiz6 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SkinStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quiz7 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcneStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResultQuiz", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResultQuiz_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Routine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResultQuizId = table.Column<int>(type: "int", nullable: false),
                    StepOrder = table.Column<int>(type: "int", nullable: false),
                    Instruction = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Routine_ResultQuiz_ResultQuizId",
                        column: x => x.ResultQuizId,
                        principalTable: "ResultQuiz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecommendProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    RecommendReason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoutineId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecommendProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecommendProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecommendProduct_Routine_RoutineId",
                        column: x => x.RoutineId,
                        principalTable: "Routine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecommendProduct_ProductId",
                table: "RecommendProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_RecommendProduct_RoutineId",
                table: "RecommendProduct",
                column: "RoutineId");

            migrationBuilder.CreateIndex(
                name: "IX_ResultQuiz_UserId",
                table: "ResultQuiz",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Routine_ResultQuizId",
                table: "Routine",
                column: "ResultQuizId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecommendProduct");

            migrationBuilder.DropTable(
                name: "Routine");

            migrationBuilder.DropTable(
                name: "ResultQuiz");
        }
    }
}
