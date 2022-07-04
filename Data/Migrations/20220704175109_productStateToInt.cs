using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StepEbay.Data.Migrations
{
    public partial class productStateToInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductStates_ProductStateId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductStates",
                table: "ProductStates");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductStateId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductStates");

            migrationBuilder.DropColumn(
                name: "ProductStateId",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Id",
                table: "ProductStates",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "ProductStateId",
                table: "Products",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductStates",
                table: "ProductStates",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductStateId",
                table: "Products",
                column: "ProductStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductStates_ProductStateId",
                table: "Products",
                column: "ProductStateId",
                principalTable: "ProductStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
