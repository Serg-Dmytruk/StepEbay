using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StepEbay.Data.Migrations
{
    public partial class Fix_Entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_NickName",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_NickName",
                table: "Users",
                column: "NickName",
                unique: true,
                filter: "[NickName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_NickName",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_NickName",
                table: "Users",
                column: "NickName");
        }
    }
}
