using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StepEbay.Data.Migrations
{
    public partial class AddPurchaseState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PurchaseStateId",
                table: "Purchases",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateTable(
                name: "PurchaseStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseStates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_PurchaseStateId",
                table: "Purchases",
                column: "PurchaseStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_PurchaseStates_PurchaseStateId",
                table: "Purchases",
                column: "PurchaseStateId",
                principalTable: "PurchaseStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_PurchaseStates_PurchaseStateId",
                table: "Purchases");

            migrationBuilder.DropTable(
                name: "PurchaseStates");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_PurchaseStateId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "PurchaseStateId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Products");
        }
    }
}
