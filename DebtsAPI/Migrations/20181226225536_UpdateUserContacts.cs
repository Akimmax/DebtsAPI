using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtsAPI.Migrations
{
    public partial class UpdateUserContacts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserContacts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "UserContacts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserContacts");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "UserContacts");
        }
    }
}
