using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StepEbay.Data.Migrations
{
    public partial class BetModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateClose",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseTypeId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PurchaseType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_PurchaseTypeId",
                table: "Products",
                column: "PurchaseTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_PurchaseType_PurchaseTypeId",
                table: "Products",
                column: "PurchaseTypeId",
                principalTable: "PurchaseType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_PurchaseType_PurchaseTypeId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "PurchaseType");

            migrationBuilder.DropIndex(
                name: "IX_Products_PurchaseTypeId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DateClose",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PurchaseTypeId",
                table: "Products");
        }
    }
}
