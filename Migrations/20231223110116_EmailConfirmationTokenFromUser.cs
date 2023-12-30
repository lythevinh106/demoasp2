using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace demoAsp2.Migrations
{
    public partial class EmailConfirmationTokenFromUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EmailConfirmationToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "productOrders",
                keyColumns: new[] { "orderId", "productId" },
                keyValues: new object[] { 1, 1 },
                column: "quantity",
                value: 3);

            migrationBuilder.UpdateData(
                table: "productOrders",
                keyColumns: new[] { "orderId", "productId" },
                keyValues: new object[] { 2, 2 },
                column: "quantity",
                value: 10);

            migrationBuilder.UpdateData(
                table: "productOrders",
                keyColumns: new[] { "orderId", "productId" },
                keyValues: new object[] { 3, 3 },
                column: "quantity",
                value: 6);

            migrationBuilder.UpdateData(
                table: "productOrders",
                keyColumns: new[] { "orderId", "productId" },
                keyValues: new object[] { 4, 4 },
                column: "quantity",
                value: 22);

            migrationBuilder.UpdateData(
                table: "productOrders",
                keyColumns: new[] { "orderId", "productId" },
                keyValues: new object[] { 5, 5 },
                column: "quantity",
                value: 33);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EmailConfirmationToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "productOrders",
                keyColumns: new[] { "orderId", "productId" },
                keyValues: new object[] { 1, 1 },
                column: "quantity",
                value: 22);

            migrationBuilder.UpdateData(
                table: "productOrders",
                keyColumns: new[] { "orderId", "productId" },
                keyValues: new object[] { 2, 2 },
                column: "quantity",
                value: 23);

            migrationBuilder.UpdateData(
                table: "productOrders",
                keyColumns: new[] { "orderId", "productId" },
                keyValues: new object[] { 3, 3 },
                column: "quantity",
                value: 18);

            migrationBuilder.UpdateData(
                table: "productOrders",
                keyColumns: new[] { "orderId", "productId" },
                keyValues: new object[] { 4, 4 },
                column: "quantity",
                value: 44);

            migrationBuilder.UpdateData(
                table: "productOrders",
                keyColumns: new[] { "orderId", "productId" },
                keyValues: new object[] { 5, 5 },
                column: "quantity",
                value: 36);
        }
    }
}
