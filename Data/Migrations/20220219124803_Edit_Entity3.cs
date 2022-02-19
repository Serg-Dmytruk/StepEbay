using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StepEbay.Data.Migrations
{
    public partial class Edit_Entity3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Value",
                table: "RefreshTokens",
                newName: "Token");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Token",
                table: "RefreshTokens",
                newName: "Value");
        }
    }
}
