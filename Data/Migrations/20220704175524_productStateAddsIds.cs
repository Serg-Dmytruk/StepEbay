using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StepEbay.Data.Migrations
{
    public partial class productStateAddsIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ProductStates",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ProductStateId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
