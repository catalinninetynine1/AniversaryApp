using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AniversaryApp.Migrations
{
    public partial class UserEmailUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserEmailA",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmailA",
                table: "User");
        }
    }
}
