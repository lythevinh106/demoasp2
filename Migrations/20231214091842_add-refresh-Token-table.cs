using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace demoAsp2.Migrations
{
    public partial class addrefreshTokentable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "refreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_refreshToken_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "productOrders",
                keyColumns: new[] { "orderId", "productId" },
                keyValues: new object[] { 1, 1 },
                column: "quantity",
                value: 28);

            migrationBuilder.UpdateData(
                table: "productOrders",
                keyColumns: new[] { "orderId", "productId" },
                keyValues: new object[] { 2, 2 },
                column: "quantity",
                value: 16);

            migrationBuilder.UpdateData(
                table: "productOrders",
                keyColumns: new[] { "orderId", "productId" },
                keyValues: new object[] { 3, 3 },
                column: "quantity",
                value: 8);

            migrationBuilder.UpdateData(
                table: "productOrders",
                keyColumns: new[] { "orderId", "productId" },
                keyValues: new object[] { 4, 4 },
                column: "quantity",
                value: 34);

            migrationBuilder.UpdateData(
                table: "productOrders",
                keyColumns: new[] { "orderId", "productId" },
                keyValues: new object[] { 5, 5 },
                column: "quantity",
                value: 42);

            migrationBuilder.CreateIndex(
                name: "IX_refreshToken_UserId",
                table: "refreshToken",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "refreshToken");

            migrationBuilder.UpdateData(
                table: "productOrders",
                keyColumns: new[] { "orderId", "productId" },
                keyValues: new object[] { 1, 1 },
                column: "quantity",
                value: 16);

            migrationBuilder.UpdateData(
                table: "productOrders",
                keyColumns: new[] { "orderId", "productId" },
                keyValues: new object[] { 2, 2 },
                column: "quantity",
                value: 30);

            migrationBuilder.UpdateData(
                table: "productOrders",
                keyColumns: new[] { "orderId", "productId" },
                keyValues: new object[] { 3, 3 },
                column: "quantity",
                value: 21);

            migrationBuilder.UpdateData(
                table: "productOrders",
                keyColumns: new[] { "orderId", "productId" },
                keyValues: new object[] { 4, 4 },
                column: "quantity",
                value: 48);

            migrationBuilder.UpdateData(
                table: "productOrders",
                keyColumns: new[] { "orderId", "productId" },
                keyValues: new object[] { 5, 5 },
                column: "quantity",
                value: 8);
        }
    }
}
