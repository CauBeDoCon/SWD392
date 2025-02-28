using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SWD392.Migrations
{
    /// <inheritdoc />
    public partial class AddShippingReturn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShippingMethod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingMethod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shipping",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShippingAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstimatedDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShippingCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingMethodId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shipping_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId");
                    table.ForeignKey(
                        name: "FK_Shipping_ShippingMethod_ShippingMethodId",
                        column: x => x.ShippingMethodId,
                        principalTable: "ShippingMethod",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Return",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Return", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Return_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId");
                    table.ForeignKey(
                        name: "FK_Return_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Return_Shipping_ShippingId",
                        column: x => x.ShippingId,
                        principalTable: "Shipping",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Return_OrderId",
                table: "Return",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Return_ProductId",
                table: "Return",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Return_ShippingId",
                table: "Return",
                column: "ShippingId",
                unique: true,
                filter: "[ShippingId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Shipping_OrderId",
                table: "Shipping",
                column: "OrderId",
                unique: true,
                filter: "[OrderId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Shipping_ShippingMethodId",
                table: "Shipping",
                column: "ShippingMethodId",
                unique: true,
                filter: "[ShippingMethodId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Return");

            migrationBuilder.DropTable(
                name: "Shipping");

            migrationBuilder.DropTable(
                name: "ShippingMethod");
        }
    }
}
