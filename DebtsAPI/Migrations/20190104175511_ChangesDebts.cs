using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DebtsAPI.Migrations
{
    public partial class ChangesDebts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Deadline",
                table: "Debts",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Debts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "Debts");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Debts");
        }
    }
}
