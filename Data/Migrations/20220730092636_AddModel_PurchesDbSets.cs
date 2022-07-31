using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StepEbay.Data.Migrations
{
    public partial class AddModel_PurchesDbSets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_PurchaseType_PurchaseTypeId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseType",
                table: "PurchaseType");

            migrationBuilder.RenameTable(
                name: "PurchaseType",
                newName: "PurchaseTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseTypes",
                table: "PurchaseTypes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PoductId = table.Column<int>(type: "int", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchases_Products_PoductId",
                        column: x => x.PoductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Purchases_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_PoductId",
                table: "Purchases",
                column: "PoductId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_UserId",
                table: "Purchases",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_PurchaseTypes_PurchaseTypeId",
                table: "Products",
                column: "PurchaseTypeId",
                principalTable: "PurchaseTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_PurchaseTypes_PurchaseTypeId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PurchaseTypes",
                table: "PurchaseTypes");

            migrationBuilder.RenameTable(
                name: "PurchaseTypes",
                newName: "PurchaseType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PurchaseType",
                table: "PurchaseType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_PurchaseType_PurchaseTypeId",
                table: "Products",
                column: "PurchaseTypeId",
                principalTable: "PurchaseType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
